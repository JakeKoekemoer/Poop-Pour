# Theme Styles

This directory contains all the theme-related CSS files for the Poop & Pour application.

## Structure

```
styles/
├── index.css                  # Main entry point - imports Tailwind and all themes
└── themes/                    # Individual theme files
    ├── soft-natural.css       # 🌿 Default theme (Calm + Modern)
    ├── fresh-cheerful.css     # 🌞 Bright and clean
    ├── warm-modern.css        # 🌼 Design-forward
    ├── ocean-calm.css         # 🌊 Cool and trustworthy
    ├── midnight-luxe.css      # 🌙 Premium dark theme
    ├── berry-soft.css         # 🍓 Playful and warm
    ├── frosted-mint.css       # 🧊 Clean and minimal
    ├── sunset-pop.css         # 🌅 Bold and energetic
    └── cozy-earth.css         # 🧸 Natural and calm
```

## Adding a New Theme

1. Create a new CSS file in `themes/` (e.g., `my-theme.css`)
2. Define your color variables for both light and dark modes
3. Import it in `index.css`
4. Add the theme to `src/types/theme.ts`
5. Update the theme store validation in `src/stores/theme.ts`

## Theme Variables

Each theme should define:

### Core Colors
- `--primary` / `--primary-foreground`
- `--secondary` / `--secondary-foreground`
- `--accent` / `--accent-foreground`
- `--background` / `--foreground`

### Components
- `--card` / `--card-foreground`
- `--popover` / `--popover-foreground`
- `--muted` / `--muted-foreground`

### Form Elements
- `--border`
- `--input`
- `--ring`

### States
- `--destructive` / `--destructive-foreground`
- `--success` / `--success-foreground`

### Optional (for advanced components)
- `--chart-1` through `--chart-5`
- `--sidebar-*` (8 variables)

## Usage

Themes are automatically loaded when you import `./styles/index.css` in your main.ts file.

The active theme is controlled by the `data-theme` attribute on the `:root` element and the `.dark` class for dark mode.
