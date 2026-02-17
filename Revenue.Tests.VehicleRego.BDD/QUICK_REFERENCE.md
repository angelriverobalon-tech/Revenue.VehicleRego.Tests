# 🚀 Quick Reference - CI/CD Pipeline

## GitHub Actions - Quick Commands

### View Pipeline Status
```
🌐 https://github.com/angelriverobalon-tech/Revenue.VehicleRego.Tests/actions
```

### Trigger Manual Run
1. Go to Actions tab
2. Select "Test Automation CI/CD"
3. Click "Run workflow"
4. Select branch → Run

### Add Status Badge to Docs
```markdown
[![Test Status](https://github.com/angelriverobalon-tech/Revenue.VehicleRego.Tests/actions/workflows/test-automation.yml/badge.svg)](https://github.com/angelriverobalon-tech/Revenue.VehicleRego.Tests/actions)
```

---

## Pipeline Triggers

| Trigger | GitHub Actions | Jenkins |
|---------|----------------|---------|
| **Push to main** | ✅ Automatic | ✅ Automatic |
| **Pull Request** | ✅ Automatic | ✅ Automatic |
| **Manual** | ✅ Workflow dispatch | ✅ Build now |
| **Scheduled** | ✅ Daily 9 AM UTC | ⚙️ Configurable |

---

## Test Commands (Local)

```bash
# Run all tests
dotnet test

# Run with HTML report
dotnet test --logger "html;LogFileName=report.html"

# List all tests
dotnet test --list-tests

# Run specific test
dotnet test --filter "TestCategory=API"

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"
```

---

## Pipeline Files

| File | Purpose |
|------|---------|
| `.github/workflows/test-automation.yml` | GitHub Actions pipeline |
| `Jenkinsfile` | Jenkins pipeline |
| `reqnroll.json` | Reqnroll configuration |
| `Revenue.Tests.BDD.csproj` | Project file with dependencies |

---

## Common Issues & Fixes

### ❌ Playwright not installing
```bash
# Manual install
pwsh Scripts/install-playwright.ps1
playwright install --with-deps
```

### ❌ Tests not found
```bash
# Rebuild the project
dotnet clean
dotnet build
```

### ❌ Pipeline failing on GitHub Actions
- Check Actions tab → View logs
- Verify all files are committed
- Check secrets (if any)

---

## Test Results Locations

### After GitHub Actions Run
- **Artifacts** → Download `test-results.zip`
- **Summary** → View inline test results
- **Logs** → Check each step for errors

### After Local Run
```
/TestResults/
  ├── test-results.trx    (Machine-readable)
  └── test-results.html   (Human-readable)
```

---

## Key URLs

| Resource | URL |
|----------|-----|
| **Repo** | https://github.com/angelriverobalon-tech/Revenue.VehicleRego.Tests |
| **Actions** | /actions |
| **Latest Run** | /actions/workflows/test-automation.yml |
| **Reqnroll Docs** | https://docs.reqnroll.net/ |
| **Playwright Docs** | https://playwright.dev/dotnet/ |

---

## Demo Checklist

- [ ] Code pushed to GitHub
- [ ] GitHub Actions enabled
- [ ] README updated with badge
- [ ] Sample test run completed
- [ ] Artifacts downloadable
- [ ] Screenshots prepared
- [ ] Know where logs are

---

## One-Liner Setup

```bash
# Commit everything and push
git add . && git commit -m "Add CI/CD pipeline" && git push origin main
```

That's it! Pipeline will auto-run on push. ✨

---

## Support Files in This Repo

📄 **README.md** - Full documentation  
📄 **DEMO_GUIDE.md** - Step-by-step demo script  
📄 **CI_CD_COMPARISON.md** - GitHub Actions vs Jenkins  
📄 **This file** - Quick reference  

---

**Last Updated:** 2024
**Pipeline Status:** ✅ Active on GitHub Actions
