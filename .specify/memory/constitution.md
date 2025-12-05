<!--
================================================================================
SYNC IMPACT REPORT
================================================================================
Version Change: N/A → 1.0.0 (Initial Release)
Modified Principles: N/A (Initial)
Added Sections:
  - Core Principles (4 principles)
  - Quality Gates
  - Development Workflow
  - Governance
Removed Sections: N/A
Templates Requiring Updates:
  - .specify/templates/plan-template.md: ✅ Compatible (Constitution Check section exists)
  - .specify/templates/spec-template.md: ✅ Compatible (Success Criteria section exists)
  - .specify/templates/tasks-template.md: ✅ Compatible (Phase structure supports principles)
Follow-up TODOs: None
================================================================================
-->

# Mortgage Planner Constitution

## Core Principles

### I. Code Quality (NON-NEGOTIABLE)

All code MUST adhere to established quality standards to ensure maintainability, readability, and long-term project health.

**Requirements:**

- Code MUST follow consistent naming conventions and formatting standards
- All functions/methods MUST have clear, single responsibilities (Single Responsibility Principle)
- Code MUST be self-documenting; comments explain "why", not "what"
- Magic numbers and hardcoded strings MUST be replaced with named constants
- Code duplication MUST be eliminated through appropriate abstractions (DRY principle)
- All public APIs MUST have JSDoc/TSDoc documentation
- Cyclomatic complexity per function MUST NOT exceed 10
- Files MUST NOT exceed 300 lines; functions MUST NOT exceed 50 lines

**Rationale:** High code quality reduces technical debt, accelerates onboarding, and minimizes bug introduction during feature development.

### II. Testing Standards (NON-NEGOTIABLE)

Testing is mandatory and MUST follow a structured approach to ensure reliability and prevent regressions.

**Requirements:**

- Unit test coverage MUST be at least 80% for all business logic
- All new features MUST include corresponding test cases before PR approval
- Test-Driven Development (TDD) is RECOMMENDED: write tests first, then implement
- Integration tests MUST cover all API endpoints and critical user flows
- Tests MUST be independent, deterministic, and isolated (no shared state)
- Test naming MUST follow pattern: `should_[expected]_when_[condition]`
- Mocking MUST be used for external dependencies and I/O operations
- CI pipeline MUST fail if any test fails or coverage drops below threshold

**Rationale:** Comprehensive testing provides confidence for refactoring, documents expected behavior, and catches regressions early in the development cycle.

### III. User Experience Consistency

The application MUST provide a consistent, intuitive, and accessible user experience across all interfaces.

**Requirements:**

- All UI components MUST follow a unified design system and component library
- Response feedback MUST be provided within 100ms for user interactions
- Error messages MUST be user-friendly, actionable, and localized
- Loading states MUST be displayed for operations exceeding 200ms
- Navigation patterns MUST be consistent across all pages/screens
- Form validation MUST provide real-time feedback
- Accessibility MUST comply with WCAG 2.1 Level AA standards
- Mobile responsiveness MUST be implemented for all user-facing features

**Rationale:** Consistent UX builds user trust, reduces learning curve, and improves overall satisfaction with the application.

### IV. Performance Requirements

The application MUST meet defined performance targets to ensure optimal user experience and system efficiency.

**Requirements:**

- Initial page load (First Contentful Paint) MUST complete within 1.5 seconds
- Time to Interactive (TTI) MUST be under 3 seconds
- API response times MUST be under 200ms for p95 percentile
- Database queries MUST complete within 100ms; complex queries within 500ms
- Bundle size MUST NOT exceed 250KB gzipped for initial load
- Memory leaks MUST be prevented; heap usage must remain stable over time
- Lighthouse Performance score MUST be at least 90
- Core Web Vitals MUST meet "Good" thresholds (LCP < 2.5s, FID < 100ms, CLS < 0.1)

**Rationale:** Performance directly impacts user retention, SEO rankings, and operational costs. Slow applications lead to user abandonment.

## Quality Gates

All code changes MUST pass through these gates before merging:

| Gate | Criteria | Enforcement |
|------|----------|-------------|
| Linting | Zero errors, zero warnings | CI automated |
| Type Safety | Full TypeScript strict mode compliance | CI automated |
| Unit Tests | All pass, coverage ≥ 80% | CI automated |
| Integration Tests | All pass for affected features | CI automated |
| Performance | Lighthouse ≥ 90, no bundle regression | CI automated |
| Code Review | At least 1 approval required | GitHub required review |
| Accessibility | No new a11y violations | CI automated |

## Development Workflow

### Feature Development Process

1. **Specification**: Create feature spec using `/speckit.spec` command
2. **Planning**: Generate implementation plan using `/speckit.plan` command
3. **Task Breakdown**: Create tasks using `/speckit.tasks` command
4. **Implementation**: Follow task phases with constitution checks
5. **Review**: Verify compliance with all quality gates
6. **Merge**: Squash merge with conventional commit message

### Code Review Requirements

- All PRs MUST reference the related spec/plan documents
- Reviewers MUST verify constitution compliance
- Performance implications MUST be documented for significant changes
- Breaking changes MUST include migration documentation

## Governance

This Constitution supersedes all other development practices within the project. All team members and contributors MUST adhere to these principles.

### Amendment Process

1. Propose amendment with rationale in a dedicated PR
2. Document impact on existing code and migration plan
3. Obtain approval from project maintainers
4. Update version following semantic versioning:
   - **MAJOR**: Principle removal or incompatible redefinition
   - **MINOR**: New principle or material expansion
   - **PATCH**: Clarifications and non-semantic refinements
5. Communicate changes to all contributors

### Compliance

- All PRs/code reviews MUST verify constitutional compliance
- Violations MUST be documented with justification if unavoidable
- Repeated violations without justification may result in PR rejection

**Version**: 1.0.0 | **Ratified**: 2025-12-05 | **Last Amended**: 2025-12-05
