<script setup lang="ts">
interface Props {
  label: string
  variant?: 'primary' | 'ghost'
  type?: 'button' | 'submit' | 'reset'
  loading?: boolean
  disabled?: boolean
  icon?: string
}

withDefaults(defineProps<Props>(), {
  variant: 'primary',
  type: 'button',
  loading: false,
  disabled: false,
})
</script>

<template>
  <button
    :type="type"
    :disabled="disabled || loading"
    :class="[
      'relative group overflow-hidden w-full py-3.5 px-6 rounded-lg font-label-md font-semibold text-sm transition-all duration-300 flex items-center justify-center gap-2 select-none outline-none focus:ring-2 focus:ring-primary/50',
      variant === 'primary'
        ? 'bg-primary-container text-white shadow-lg shadow-primary-container/25 hover:shadow-primary-container/40 hover:-translate-y-0.5 active:translate-y-0'
        : 'bg-surface-bright/40 border border-surface-bright hover:border-on-surface/40 text-on-surface hover:bg-surface-bright/70',
      disabled || loading ? 'opacity-60 cursor-not-allowed transform-none shadow-none' : '',
    ]"
  >
    <span
      v-if="variant === 'primary' && !disabled && !loading"
      class="absolute inset-0 bg-white/15 translate-y-full group-hover:translate-y-0 transition-transform duration-300 ease-out pointer-events-none"
    />

    <span
      v-if="loading"
      class="inline-block w-4 h-4 border-2 border-current border-t-transparent rounded-full animate-spin"
    />

    <span class="relative z-10 font-bold tracking-wide">
      {{ loading ? 'Verificando...' : label }}
    </span>

    <span
      v-if="icon && !loading"
      class="material-symbols-outlined relative z-10 text-lg group-hover:translate-x-0.5 transition-transform duration-200"
    >
      {{ icon }}
    </span>
  </button>
</template>
