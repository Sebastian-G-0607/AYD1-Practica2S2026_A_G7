<script setup lang="ts">
import { ref, watch, computed, onMounted } from 'vue'
import { sharesService } from '@/services/shares.service'
import { getResenias } from '@/services/resenia.service'
import type { ShareUser } from '@/types/shares.types'
import type { Resenia } from '@/types/resenia.types'
import { useAuthStore } from '@/stores/auth.store'

interface Props {
  isOpen: boolean
  reseniaId?: number
  reseniaTitle?: string
}

const props = withDefaults(defineProps<Props>(), {
  isOpen: false,
  reseniaId: undefined,
  reseniaTitle: '',
})

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'share', payload: { reseniaId: number; userIds: number[] }): void
}>()

const authStore = useAuthStore()

const searchQuery = ref('')
const usersList = ref<ShareUser[]>([])
const loadingUsers = ref(false)
const selectedUsers = ref<number[]>([])

// Lista de reseñas del usuario en caso de que no se haya preseleccionado una
const userResenias = ref<Resenia[]>([])
const loadingResenias = ref(false)
const selectedReseniaId = ref<number | null>(null)

const currentReseniaId = computed(() => {
  return props.reseniaId || selectedReseniaId.value || 0
})

const filteredUsers = computed(() => {
  if (!searchQuery.value.trim()) return usersList.value
  const q = searchQuery.value.toLowerCase().trim()
  return usersList.value.filter(
    (u) => u.nombre.toLowerCase().includes(q) || u.correo.toLowerCase().includes(q),
  )
})

const getUserInitials = (nombre: string) => {
  const parts = nombre.trim().split(/\s+/)
  const first = parts[0]
  const second = parts[1]
  if (first && second && first.length > 0 && second.length > 0) {
    return (first.charAt(0) + second.charAt(0)).toUpperCase()
  }
  return nombre.substring(0, 2).toUpperCase() || 'U'
}

async function loadUsers() {
  try {
    loadingUsers.value = true
    const currentUserId = authStore.user?.id
    usersList.value = await sharesService.getUsers(undefined, currentUserId)
  } catch (err) {
    console.error('Error al cargar usuarios:', err)
  } finally {
    loadingUsers.value = false
  }
}

async function loadMyResenias() {
  try {
    loadingResenias.value = true
    userResenias.value = await getResenias()
    const firstResenia = userResenias.value[0]
    if (firstResenia && !selectedReseniaId.value) {
      selectedReseniaId.value = firstResenia.id
    }
  } catch (err) {
    console.error('Error al cargar reseñas del usuario:', err)
  } finally {
    loadingResenias.value = false
  }
}

watch(
  () => props.isOpen,
  (newVal) => {
    if (newVal) {
      selectedUsers.value = []
      searchQuery.value = ''
      loadUsers()

      if (!props.reseniaId) {
        loadMyResenias()
      } else {
        selectedReseniaId.value = props.reseniaId
      }
    }
  },
  { immediate: true },
)

const handleShare = () => {
  if (!currentReseniaId.value || selectedUsers.value.length === 0) return

  emit('share', {
    reseniaId: currentReseniaId.value,
    userIds: selectedUsers.value,
  })
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
      class="bg-surface-container-high w-full max-w-lg rounded-2xl shadow-2xl relative z-10 flex flex-col max-h-[85vh] transform transition-transform duration-300 overflow-hidden border border-outline-variant/20"
      :class="isOpen ? 'scale-100' : 'scale-95'"
    >
      <!-- Cabecera -->
      <div class="px-6 py-5 border-b border-outline-variant/10 flex justify-between items-center bg-surface-container-high">
        <div>
          <h2 class="font-headline-md text-headline-md text-on-surface">Compartir Reseña</h2>
          <p class="text-caption font-caption text-on-surface-variant mt-1">
            Selecciona los miembros de la comunidad con quienes compartir tu opinión.
          </p>
        </div>
        <button
          @click="emit('close')"
          class="p-2 rounded-full hover:bg-surface-variant text-on-surface-variant transition-colors"
        >
          <span class="material-symbols-outlined">close</span>
        </button>
      </div>

      <!-- Selector de Reseña (si no se pasó una reseña fija por prop) -->
      <div v-if="!reseniaId" class="px-6 py-4 border-b border-outline-variant/10 bg-surface-container/60">
        <label class="block font-label-md text-label-md text-on-surface mb-2 font-semibold">
          Selecciona la reseña a compartir:
        </label>
        <div v-if="loadingResenias" class="text-sm text-on-surface-variant py-2">
          Cargando tus reseñas...
        </div>
        <div v-else-if="userResenias.length === 0" class="text-sm text-yellow-400 py-2">
          No tienes reseñas registradas para compartir. Crea una primero.
        </div>
        <select
          v-else
          v-model="selectedReseniaId"
          class="w-full bg-surface-dim border border-outline-variant/20 rounded-xl px-4 py-2.5 text-on-surface text-sm focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary"
        >
          <option v-for="res in userResenias" :key="res.id" :value="res.id">
            {{ res.tituloPelicula }} ({{ '★'.repeat(res.calificacion) }})
          </option>
        </select>
      </div>

      <div v-else class="px-6 py-3 border-b border-outline-variant/10 bg-surface-container/40 flex items-center gap-2">
        <span class="material-symbols-outlined text-primary text-sm">movie</span>
        <span class="text-xs text-on-surface-variant">Compartiendo:</span>
        <span class="text-sm font-semibold text-on-surface">{{ reseniaTitle || `Reseña #${reseniaId}` }}</span>
      </div>

      <!-- Buscador -->
      <div class="p-4 border-b border-outline-variant/10 bg-surface-container/50">
        <div class="relative group">
          <span class="material-symbols-outlined absolute left-4 top-1/2 -translate-y-1/2 text-on-surface-variant group-focus-within:text-primary transition-colors">search</span>
          <input
            v-model="searchQuery"
            class="w-full bg-surface-variant/50 border border-outline-variant/20 rounded-xl py-3 pl-12 pr-6 text-on-surface placeholder:text-on-surface-variant/50 focus:outline-none focus:ring-2 focus:ring-primary/30 focus:border-primary transition-all font-body-md"
            placeholder="Buscar por nombre o correo electrónico..."
            type="text"
          />
        </div>
      </div>

      <!-- Lista de Usuarios Dinámica -->
      <div class="flex-1 overflow-y-auto p-3 space-y-1 min-h-[200px]">
        <div v-if="loadingUsers" class="flex items-center justify-center py-8 text-on-surface-variant text-sm gap-2">
          <span class="material-symbols-outlined animate-spin">sync</span>
          Cargando usuarios registrados...
        </div>

        <div v-else-if="filteredUsers.length === 0" class="text-center py-8 text-on-surface-variant text-sm">
          No se encontraron usuarios registrados disponibles.
        </div>

        <label
          v-for="user in filteredUsers"
          :key="user.id"
          class="flex items-center gap-4 p-3 rounded-xl hover:bg-surface-variant/50 cursor-pointer transition-colors group"
        >
          <div class="relative flex items-center">
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
          <div class="w-10 h-10 rounded-full bg-primary/10 text-primary flex items-center justify-center font-bold text-sm">
            {{ getUserInitials(user.nombre) }}
          </div>
          <div class="flex-1 min-w-0">
            <div class="font-label-md text-sm text-on-surface truncate group-hover:text-primary transition-colors font-semibold">
              {{ user.nombre }}
            </div>
            <div class="font-caption text-caption text-on-surface-variant truncate">
              {{ user.correo }}
            </div>
          </div>
          <span class="text-xs px-2 py-0.5 rounded bg-surface-variant text-on-surface-variant font-mono uppercase">
            {{ user.rol }}
          </span>
        </label>
      </div>

      <!-- Footer con contador y botón compartir -->
      <div class="p-5 border-t border-outline-variant/10 bg-surface-container-high flex justify-between items-center rounded-b-2xl">
        <div class="text-sm font-body-md text-on-surface-variant">
          <span class="font-bold text-on-surface">{{ selectedUsers.length }}</span> usuario(s) seleccionado(s)
        </div>
        <button
          @click="handleShare"
          :disabled="selectedUsers.length === 0 || !currentReseniaId"
          class="bg-primary text-on-primary px-8 py-2.5 rounded-full font-label-md text-label-md hover:bg-primary-fixed transition-all shadow-md shadow-primary/20 hover:shadow-primary/40 focus:ring-2 focus:ring-primary focus:ring-offset-2 focus:ring-offset-surface-container-high disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-2"
        >
          <span class="material-symbols-outlined text-sm">share</span>
          <span>Compartir</span>
        </button>
      </div>
    </div>
  </div>
</template>