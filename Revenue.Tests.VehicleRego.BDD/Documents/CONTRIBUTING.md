# Contributing to Revenue.VehicleRego.Tests!

We're glad you're interested in contributing to Revenue.VehicleRego.Tests!, Here are some guidelines to help you get started.

## Submitting a Pull Request

Before you submit a pull request, please make sure to do the following:

1. **Clone** the repository to your local machine.
2. **Create a new branch** for your changes.(Example: `<#issueNo>-<branchName>`)
3. **Make the necessary changes** and **commit** them to your new branch.
4. **Push** your new branch to your repository on Azure Repository.
5. **Create a new pull request** against the `main` branch of the original repository.

When creating the pull request, please make sure to include the following:

- **A clear and descriptive title** that summarizes the changes you're making.(Example:`<type>(<scope>): <subject>-<#issueNo>`)
- **A detailed description** of the changes you're making, including any relevant context or background information.
- **Screenshots or GIFs** (if applicable) that show the before and after of the changes.
- **Links to any related issues** or pull requests.

_Please make sure to follow the [guidelines](https://www.pullrequest.com/blog/writing-a-great-pull-request-description/) for writing a great pull request description._

## Committing changes

We try to follow the [Semantic Commit Message](https://seesparkbox.com/foundry/semantic_commit_messages) conventions for committing changes. This means that commit messages should have the following format:

`<type>(<scope>): <subject>`

Where:

- `type` is a word that describes the type of change being made, such as `feat`, `fix`, `docs`, `style`, `refactor`, `test`, or `chore`.
- `scope` is an optional word that describes the specific part of the project that the change relates to, such as a module or component name.
- `subject` is a brief summary of the change, written in the present tense.

### Examples

Here are some examples of commit messages and pull request names, following this format:

| Description                     | Type       | Scope      | Commit Message                                                                                   |
| ------------------------------- | ---------- | ---------- | ------------------------------------------------------------------------------------------------ |
| Test data set up for a scenario | `data`     | `purchase` | `data(purchase): set up test data for purchase scenario`                                         |
| Adding a Scenario               | `scenario` | `purchase` | `scenario(purchase): added a new scenario to the purchase module`                                |
| Fixing a Scenario               | `fix`      | `purchase` | `fix(purchase): fixed a bug in the purchase scenario`                                            |
| Refactoring a Scenario          | `refactor` | `purchase` | `refactor(purchase): refactored the purchase scenario for better code structure and readability` |
| Commentary for a Scenario       | `docs`     | `purchase` | `docs(purchase): added detailed commentary to the purchase scenario`                             |
| Styling for a Scenario          | `style`    | `purchase` | `style(purchase): improved the formatting and styling of the purchase scenario`                  |
| Adding a feature                | `feat`     | `solution` | `feat(solution): added a new feature to the solution`                                            |
| Fixing a component of solution  | `fix`      | `solution` | `fix(solution): fixed a bug in the solution`                                                     |
| Refactoring the solution        | `refactor` | `solution` | `refactor(solution): refactored the solution for better code structure and readability`          |
| Commentary for the solution     | `docs`     | `solution` | `docs(solution): added detailed commentary to the solution`                                      |
| Styling for the solution        | `style`    | `solution` | `style(solution): improved the formatting and styling of the solution`                           |

Additional examples

- chore: add Oyster build script
- test: ensure Tayne retains clothing
