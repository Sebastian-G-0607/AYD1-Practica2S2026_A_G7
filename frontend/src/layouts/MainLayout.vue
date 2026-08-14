<script setup lang="ts">
import { ref } from 'vue'
import Sidebar from '@/components/layout/Sidebar.vue'
import Header from '@/components/layout/Header.vue'

const isSidebarOpen = ref(false)

function toggleSidebar() {
  isSidebarOpen.value = !isSidebarOpen.value
}

function closeSidebar() {
  isSidebarOpen.value = false
}
</script>

<template>
  <div class="min-h-screen bg-surface text-on-surface flex">
    <Sidebar
      :is-open="isSidebarOpen"
      @close="closeSidebar"
    />

    <div class="flex-1 flex flex-col min-w-0 lg:pl-72 transition-all duration-300">
      <Header @toggle-sidebar="toggleSidebar" />

      <main class="flex-1 p-6 md:p-8 max-w-7xl w-full mx-auto space-y-8">
        <slot />
      </main>
    </div>
  </div>
</template>
