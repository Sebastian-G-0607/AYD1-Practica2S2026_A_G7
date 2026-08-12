interface ImportMetaEnv {
  readonly VITE_API_BASE_URL: string
  readonly VITE_APP_ENV: 'local' | 'develop' | 'main'
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}
