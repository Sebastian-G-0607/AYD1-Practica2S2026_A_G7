<script setup lang="ts">
import { RouterView } from 'vue-router'
</script>

<template>
  <div class="min-h-screen bg-surface flex items-center justify-center p-margin-mobile md:p-margin-desktop relative overflow-hidden">
    <div class="absolute -top-40 -left-40 w-96 h-96 bg-primary-container/10 rounded-full blur-3xl pointer-events-none" />
    <div class="absolute -bottom-40 -right-40 w-96 h-96 bg-secondary-container/10 rounded-full blur-3xl pointer-events-none" />

    <main class="w-full relative z-10" style="max-width: 520px;">
      <slot>
        <RouterView />
      </slot>
    </main>
  </div>
</template>
