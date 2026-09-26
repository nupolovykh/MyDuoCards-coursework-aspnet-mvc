# Dependency updates

Updates land on `deps` without a human and reach `main` through one reviewed
pull request. Built for a repository nobody is watching, so the design optimises
for "keeps working unattended" over "clever".

```
Dependabot ─▶ PR ─▶ CI ──green on this exact commit──▶ squash-merged into deps
                                                              │
                                          promotion PR (CI runs on the result)
                                                              │
                                                       ◀ human merges ▶
                                                              ▼
                                                             main
                                                              │
                                        deps re-cut from main ┘
```

Both automation workflows are byte for byte the ones running in
`QA-web-labprojects-python`, `Smart-Plan-Mortgage-Calculator` and
`Marketplace-coursework-wpf-mvvm`, where they were measured end to end against
real updates. That first repository's `docs/dependency-updates.md` carries the
design rationale and `docs/porting-the-dependency-pipeline.md` the checklist
this repository was installed from. Only what is specific here is below.

## Branch contract

| Branch | Who writes | History |
|---|---|---|
| `main` | humans only | |
| `deps` | bots only | **disposable**, re-cut from `main` after every promotion |

`deps` is never merged into — it is reset. It exists to change package versions,
`main` changes them too, and a merge-based sync leaves a human resolving those
by hand on a branch nobody watches. Reset costs nothing, because every commit on
`deps` is a bot commit and every bot commit is regenerable: reset the branch and
Dependabot raises the same bumps again on its next scan.

The guard in `deps-promote.yml` keeps that true: one non-bot commit on `deps`
and the workflow resets nothing and opens a *Dependency promotion is blocked*
issue, which it closes itself on the first run that is not blocked.

## Schedule, groups and reports

Monthly. Per directory, one pull request for every minor and patch bump and one
per major; the EntityFrameworkCore family below is its own group across all
update types, and `minor-and-patch` excludes its patterns, so its members never
land there. `security-audit.yml` writes its findings into the run
summary, never an issue. Dependabot security updates are switched off: they
target `main` directly and would bypass `deps`.

## Where `deps` came from

Not from `main`. It was cut from `security-features-main`, which held four
Dependabot merges with no route into `main` — updates landed there and stopped.
Those four are what the first promotion carries. `security-features-main` is now
dead: nothing targets it and nothing reads it.

## Why the EntityFrameworkCore packages are grouped

This project pins five of them at one version, and they carry version ranges on
each other. Bumping one alone leaves the rest below what it now demands, and the
restore either warns loudly or, where NU1605 is treated as an error, fails
outright. Ungrouped that is not one blocked update but a queue that cannot
drain, because every pull request in it breaks the same way.

That failure was measured in `Marketplace-coursework-wpf-mvvm`, which has the
identical shape. Here the group is applied before it happens rather than after.
`Microsoft.VisualStudio.Web.CodeGeneration.Design` sits in the same group
because it depends on EF Core and pulls the family forward with it.

The `>= 10.0.0` cap on `Microsoft.EntityFrameworkCore*` is a target-framework
fact, not caution: this project is `net8.0` and EF Core 10.x ships `net10.0`
assets only, so a 10.x bump cannot restore however it is grouped. It is written
as a version range rather than a major-update filter so that 8 → 9 still lands,
and scoped to EF Core alone because `Microsoft.Extensions` 10.x still ships
`net8.0`. Remove it when the project is retargeted.

## Why CI needs no change

`build-and-smoke-test.yml` runs on `push: [main]` and on `pull_request:` with no
branch filter, so a pull request into `deps` is built and a push to `deps` is
not. That is exactly what the pipeline needs and one run fewer than adding
`deps` to the push filter: the gate reads the `pull_request` run of each
Dependabot pull request, and the promotion gets its own run on the combined
result.

Two values in the gate had to match this repository and do: the workflow's
**name** (`Build & Integration Smoke Test`) in the `workflow_run` trigger, and
its **filename** (`build-and-smoke-test.yml`) in `CI_WORKFLOW`. Get either wrong
and nothing reports an error — the gate simply never fires.

## Repository settings this depends on

Not in the repository, so listed here:

1. **Secrets and variables → Actions**: `DEPS_PAT`. Without it both automation
   workflows fail immediately with 401.
2. **Actions → General → Workflow permissions**: *Allow GitHub Actions to create
   and approve pull requests* — ticked.
3. **General → Pull Requests**: squash merging enabled.
4. **Advanced Security → Dependabot alerts**: enabled. **Dependabot security
   updates**: disabled — they target `main` directly and would bypass `deps`.
5. **Branch protection on `main`**: require a pull request, and tick *Do not
   allow bypassing the above settings* — the second half is what actually stops
   a direct push by an administrator.

## Running it by hand

```
Actions → Dependency promotion → Run workflow   # realigns deps, opens the promotion PR
Actions → Dependency auto-merge → Run workflow  # sweeps; one log line per open update
```
