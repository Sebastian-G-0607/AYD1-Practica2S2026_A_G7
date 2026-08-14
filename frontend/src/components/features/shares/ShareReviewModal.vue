<script setup lang="ts">
import { ref } from 'vue'

defineProps<{
  isOpen: boolean
}>()

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'share', userIds: number[]): void
}>()

// 1. Datos simulados de los usuarios de la plataforma
const mockUsers = ref([
  { id: 1, name: 'Sarah Jenkins', handle: '@sjenkins_film', initials: 'SJ' },
  { id: 2, name: 'Marcus Thorne', handle: '@marcust_dop', initials: 'MT' },
  { id: 3, name: 'Elena Rodriguez', handle: 'elena.r@cinecraft.app', initials: 'ER' },
  { id: 4, name: 'David Chen', handle: '@dchen_edits', initials: 'DC' }
])

const selectedUsers = ref<number[]>([])

const handleShare = () => {
  emit('share', selectedUsers.value)
  selectedUsers.value = [] // Reseteamos la selección
}
</script>

<template>
  <div
    class="fixed inset-0 z-50 flex items-center justify-center transition-opacity duration-300"
    :class="isOpen ? 'opacity-100 pointer-events-auto' : 'opacity-0 pointer-events-none'"
  >
    <!-- Backdrop -->
    <div 
      class="absolute inset-0 bg-background/80 backdrop-blur-xl transition-opacity" 
      @click="emit('close')"
    ></div>
    
    <!-- Contenido del Modal -->
    <div
      class="bg-surface-container-high w-full max-w-lg rounded-2xl shadow-2xl relative z-10 flex flex-col max-h-[870px] transform transition-transform duration-300"
      :class="isOpen ? 'scale-100' : 'scale-95'"
    >
      <div class="px-6 py-5 border-b border-outline-variant/10 flex justify-between items-center">
        <div>
          <h2 class="font-headline-md text-headline-md text-on-surface">Compartir Reseña</h2>
          <p class="text-caption font-caption text-on-surface-variant mt-1">Selecciona los usuarios a los que enviar tu destacado.</p>
        </div>
        <button 
          @click="emit('close')"
          class="p-2 rounded-full hover:bg-surface-variant text-on-surface-variant transition-colors"
        >
          <span class="material-symbols-outlined">close</span>
        </button>
      </div>

      <!-- Buscador -->
      <div class="p-4 border-b border-outline-variant/10 bg-surface-container/50">
        <div class="relative group">
          <span class="material-symbols-outlined absolute left-4 top-1/2 -translate-y-1/2 text-on-surface-variant group-focus-within:text-primary transition-colors">search</span>
          <input
            class="w-full bg-surface-variant/50 border border-outline-variant/20 rounded-xl py-3 pl-12 pr-6 text-on-surface placeholder:text-on-surface-variant/50 focus:outline-none focus:ring-2 focus:ring-primary/30 focus:border-primary transition-all font-body-md"
            placeholder="Buscar por nombre o correo..." type="text" 
          />
        </div>
      </div>

      <!-- Lista de Usuarios Dinámica -->
      <div class="flex-1 overflow-y-auto p-2 space-y-1">
        <label 
          v-for="user in mockUsers" 
          :key="user.id"
          class="flex items-center gap-4 p-3 rounded-xl hover:bg-surface-variant/50 cursor-pointer transition-colors group"
        >
          <div class="relative flex items-center">
            <!-- 3. Magia de Vue: v-model une el checkbox al arreglo selectedUsers -->
            <input 
              type="checkbox" 
              :value="user.id"
              v-model="selectedUsers"
              class="peer sr-only" 
            />
            <div class="w-5 h-5 border-2 border-outline-variant rounded peer-checked:bg-primary peer-checked:border-primary transition-colors flex items-center justify-center">
              <span class="material-symbols-outlined text-[16px] text-on-primary opacity-0 peer-checked:opacity-100 scale-50 peer-checked:scale-100 transition-all duration-200">check</span>
            </div>
          </div>
          <div class="w-10 h-10 rounded-full bg-surface-variant flex items-center justify-center font-bold text-sm text-on-surface">
            {{ user.initials }}
          </div>
          <div class="flex-1 min-w-0">
            <div class="font-label-md text-sm text-on-surface truncate group-hover:text-primary transition-colors">
              {{ user.name }}
            </div>
            <div class="font-caption text-caption text-on-surface-variant truncate">
              {{ user.handle }}
            </div>
          </div>
        </label>
      </div>

      <!-- Footer con contador -->
      <div class="p-6 border-t border-outline-variant/10 bg-surface-container-high flex justify-between items-center rounded-b-2xl">
        <div class="text-sm font-body-md text-on-surface-variant">
          <!-- 4. Mostramos el tamaño del arreglo -->
          <span class="font-bold text-on-surface">{{ selectedUsers.length }}</span> usuario(s) seleccionado(s)
        </div>
        <button 
        @click="handleShare"
          :disabled="selectedUsers.length === 0"
          class="bg-primary text-on-primary px-8 py-2.5 rounded-full font-label-md text-label-md hover:bg-primary-fixed transition-all shadow-md shadow-primary/20 hover:shadow-primary/40 focus:ring-2 focus:ring-primary focus:ring-offset-2 focus:ring-offset-surface-container-high disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Compartir
        </button>
      </div>
    </div>
  </div>
</template>