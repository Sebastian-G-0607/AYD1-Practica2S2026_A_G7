
<script setup lang="ts">
import MainLayout from '@/layouts/MainLayout.vue'
import ReseniasDestacadas from '@/components/features/resenias/ReseniasDestacadas.vue'
</script>

<template>
  <MainLayout>
    <section class="bg-surface-container-low border border-surface-bright/50 rounded-2xl p-6 md:p-8 shadow-xl">
      <ReseniasDestacadas />
    </section>
  </MainLayout>
</template>