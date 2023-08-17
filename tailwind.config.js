/** @type {import('tailwindcss').Config} */

function withOpacity(variableName) {
  return ({ opacityValue }) => {
    if (opacityValue !== undefined) {
      return `rgba(var(${variableName}), ${opacityValue})`
    }
    return `rgb(var(${variableName}))`
  }
}

export default {
  cssPath: './src/style.css',
  content: ["./src/**/*.{html,vue,js}"],
  purge: ["./index.html", "./src/**/*.{vue,js,ts,jsx,tsx}"],
  darkMode: "class",
  theme: {
    fontFamily: {
      'sans': ['Mulish'],
      'body': ['Mulish']
    },
    extend: {
        textColor: {
        skin: {
          base: withOpacity("--color-text-base"),
          dark: withOpacity("--color-text-dark"),
          muted: withOpacity("--color-text-muted"),
          "muted-dark": withOpacity("--color-text-muted-dark"),
          inverted: withOpacity("--color-text-inverted"),
          accent: withOpacity("--color-text-accent"),
          error: withOpacity("--color-error"),
        },
      },
      backgroundColor: {
        skin: {
          light: withOpacity("--color-bg-light"),
          dark: withOpacity("--color-bg-dark"),
          muted: withOpacity("--color-bg-muted"),
          "muted-dark": withOpacity("--color-bg-muted-dark"),
          fill: withOpacity("--color-fill"),
          player: withOpacity("--color-bg-player"),
          "button-accent": withOpacity("--color-button-accent"),
          "button-accent-hover": withOpacity("--color-button-accent-hover"),
          "button-muted": withOpacity("--color-button-muted"),
        },
      },
      gradientColorStops: {
        skin: {
          hue: withOpacity("--color-fill"),
          from: withOpacity("--color-grad-start"),
          via: withOpacity("--color-grad-via"),
          to: withOpacity("--color-grad-end"),
        },
      },
      ringColor: {
        skin: {
          error: withOpacity("--color-error"),
          focus: withOpacity("--color-button-muted"),
          dark: withOpacity("--color-bg-muted-dark"),
          light: withOpacity("--color-bg-muted"),
        },
      },
      borderColor: {
        skin: {
          focus: withOpacity("--color-button-muted"),
          dark: withOpacity("--color-text-muted-dark"),
          light: withOpacity("--color-text-muted"),
        },
      },
      fill: {
        skin: {
          fill: withOpacity("--color-button-accent"),
        },
      },
    },
    plugins: [],
  },
  extend: {},
};
