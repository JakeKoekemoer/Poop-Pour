# Testing Checklist

## 🧪 Manual Testing Guide

### Prerequisites
1. Start dev server: `npm run dev`
2. Ensure `.env` has `VITE_SHOW_COMING_SOON=false`
3. Open browser to `http://localhost:51500`

### ✅ Navigation Testing
- [ ] Logo clicks navigate to home
- [ ] "Home" and "About" links work
- [ ] "Get Started" button navigates to login
- [ ] Navigation is fixed and stays at top when scrolling
- [ ] Mobile: Hamburger menu appears on small screens
- [ ] Mobile: Menu opens/closes properly
- [ ] Mobile: Links in menu work and close menu after click
- [ ] All hover states work properly

### ✅ Hero Section Testing
- [ ] Badge displays "✨ Trusted by 10,000+ parents"
- [ ] Heading shows with "poops" in green
- [ ] Both CTA buttons visible
- [ ] "Start Free" navigates to login
- [ ] "See How It Works" smoothly scrolls to next section
- [ ] Phone mockup placeholder displays
- [ ] Decorative emojis (🍼, 💩) visible
- [ ] Background gradient renders correctly
- [ ] Blur circles visible (subtle)

### ✅ Responsive Testing - Hero
- [ ] Mobile: Content stacks vertically
- [ ] Mobile: Typography scales down appropriately
- [ ] Tablet: Maintains good spacing
- [ ] Desktop (lg+): Side-by-side layout
- [ ] Desktop (xl): Max-width constraint applied

### ✅ How It Works Section Testing
- [ ] Section heading displays
- [ ] All 3 cards visible (Feed, Diaper, Patterns)
- [ ] Emojis display in cards (🍼, 💩, 📊)
- [ ] Number badges (01, 02, 03) visible
- [ ] Hover effect: cards lift slightly
- [ ] Hover effect: shadow increases
- [ ] Gradient backgrounds visible

### ✅ Responsive Testing - How It Works
- [ ] Mobile: Single column
- [ ] Tablet (md): 2 columns
- [ ] Desktop (lg): 3 columns
- [ ] Cards maintain aspect ratio

### ✅ Testimonials Section Testing
- [ ] "Why Parents Love It" heading displays
- [ ] 3 testimonial cards visible
- [ ] Star ratings show (5 stars, yellow)
- [ ] Testimonial text readable
- [ ] Avatars show with initials (SM, JK, ER)
- [ ] Names display correctly
- [ ] Hover effect: shadow increases

### ✅ Responsive Testing - Testimonials
- [ ] Mobile: Single column
- [ ] Tablet (md): 2 columns
- [ ] Desktop (lg): 3 columns

### ✅ Features Grid Section Testing
- [ ] Large "Built for 3AM" card spans properly
- [ ] 3 small feature cards visible:
  - [ ] ⚡ Lightning Fast
  - [ ] 🔒 Private & Secure
  - [ ] 🌙 Dark Mode Ready
- [ ] Icons display correctly (Lucide icons)
- [ ] Large card has dark gradient background
- [ ] Small cards have white background
- [ ] Hover effects work

### ✅ Responsive Testing - Features Grid
- [ ] Mobile: Stacks vertically
- [ ] Tablet+: 2-column grid with proper spans

### ✅ Final CTA Section Testing
- [ ] Gradient background displays
- [ ] Decorative emojis visible
- [ ] Heading and subheading readable
- [ ] "Start Free Today" button visible
- [ ] Button navigates to login
- [ ] Hover effect: button scales up
- [ ] Hover effect: shadow increases

### ✅ Footer Testing
- [ ] Logo and description visible
- [ ] All 4 columns display:
  - [ ] Product links
  - [ ] Company links
  - [ ] Legal links
  - [ ] Brand info with social icons
- [ ] Social media icons visible (Facebook, Twitter, Instagram, Email)
- [ ] Social icons have hover effect (scale)
- [ ] Copyright notice shows current year
- [ ] Dark sage background renders
- [ ] All links work (currently point to home)

### ✅ Responsive Testing - Footer
- [ ] Mobile: Stacks vertically
- [ ] Tablet (sm): 2x2 grid
- [ ] Desktop (lg): 4 columns

### ✅ Theme Testing
- [ ] Open theme switcher (bottom-right)
- [ ] Switch between different themes
- [ ] All colors update properly across all sections
- [ ] Toggle dark mode
- [ ] Dark mode applies correctly
- [ ] All text remains readable in dark mode

### ✅ Accessibility Testing
- [ ] Tab through all interactive elements
- [ ] Focus indicators visible
- [ ] Can navigate menu with keyboard
- [ ] Screen reader announces sections properly
- [ ] All images have alt text or aria-labels
- [ ] Color contrast is sufficient
- [ ] Headings follow proper hierarchy (h1 → h2 → h3)

### ✅ Performance Testing
- [ ] Page loads quickly
- [ ] No layout shift during load
- [ ] Smooth scrolling throughout
- [ ] Animations don't jank
- [ ] No console errors
- [ ] No linter errors

### ✅ Cross-Browser Testing
- [ ] Chrome/Edge (Chromium)
- [ ] Firefox
- [ ] Safari (if available)
- [ ] Mobile Safari (iOS)
- [ ] Mobile Chrome (Android)

### ✅ Mobile Device Testing
Test on actual devices or browser dev tools:
- [ ] iPhone SE (375px)
- [ ] iPhone 12/13/14 (390px)
- [ ] iPhone 14 Pro Max (430px)
- [ ] Android phone (360px, 412px)
- [ ] iPad (768px)
- [ ] iPad Pro (1024px)

### 🐛 Known Issues / Future Improvements
- [ ] Hero phone mockup is a placeholder (needs actual screenshot)
- [ ] "About" link goes to home (About page not created yet)
- [ ] Testimonial avatars use initials only (no images)
- [ ] Social media links go to "#" (need actual URLs)
- [ ] All footer links currently point to home

### 📝 Notes
- Environment flag `VITE_SHOW_COMING_SOON` controls which home page shows
- All components use existing theme system
- Mobile-first responsive approach used throughout
- No external dependencies added (except shadcn-vue Avatar/Sheet)
