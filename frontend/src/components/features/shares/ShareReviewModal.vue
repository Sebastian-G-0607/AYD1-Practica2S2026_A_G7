<script setup lang="ts">
import { ref, watch, computed } from 'vue'
import { sharesService } from '@/services/shares.service'
import { getResenias } from '@/services/resenia.service'
import type { ShareUser, MySharedItem } from '@/types/shares.types'
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
const hideAlreadyShared = ref(false)
const usersList = ref<ShareUser[]>([])
const mySharesList = ref<MySharedItem[]>([])
const loadingUsers = ref(false)
const selectedUsers = ref<number[]>([])

// Lista de reseñas del usuario en caso de que no se haya preseleccionado una
const userResenias = ref<Resenia[]>([])
const loadingResenias = ref(false)
const selectedReseniaId = ref<number | null>(null)

const currentReseniaId = computed(() => {
  return props.reseniaId || selectedReseniaId.value || 0
})

const alreadySharedUserIds = computed<Set<number>>(() => {
  if (!currentReseniaId.value) return new Set<number>()
  return new Set(
    mySharesList.value
      .filter((s) => s.reseniaId === currentReseniaId.value)
      .map((s) => s.usuarioDestinatarioId),
  )
})

const filteredUsers = computed(() => {
  let list = usersList.value

  if (hideAlreadyShared.value) {
    list = list.filter((u) => !alreadySharedUserIds.value.has(u.id))
  }

  if (!searchQuery.value.trim()) return list
  const q = searchQuery.value.toLowerCase().trim()
  return list.filter(
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

async function loadData() {
  try {
    loadingUsers.value = true
    const currentUserId = authStore.user?.id
    const [users, shares] = await Promise.all([
      sharesService.getUsers(undefined, currentUserId),
      sharesService.getMyShares(),
    ])
    usersList.value = users
    mySharesList.value = shares
  } catch (err) {
    console.error('Error al cargar usuarios o compartidas:', err)
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
      loadData()

      if (!props.reseniaId) {
        loadMyResenias()
      } else {
        selectedReseniaId.value = props.reseniaId
      }
    }
  },
  { immediate: true },
)

watch(currentReseniaId, () => {
  selectedUsers.value = selectedUsers.value.filter((id) => !alreadySharedUserIds.value.has(id))
})

function toggleUser(userId: number) {
  if (alreadySharedUserIds.value.has(userId)) return

  const idx = selectedUsers.value.indexOf(userId)
  if (idx > -1) {
    selectedUsers.value.splice(idx, 1)
  } else {
    selectedUsers.value.push(userId)
  }
}

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

      <!-- Buscador y Filtro -->
      <div class="p-4 border-b border-outline-variant/10 bg-surface-container/50 space-y-3">
        <div class="relative group">
          <span class="material-symbols-outlined absolute left-4 top-1/2 -translate-y-1/2 text-on-surface-variant group-focus-within:text-primary transition-colors">search</span>
          <input
            v-model="searchQuery"
            class="w-full bg-surface-variant/50 border border-outline-variant/20 rounded-xl py-3 pl-12 pr-6 text-on-surface placeholder:text-on-surface-variant/50 focus:outline-none focus:ring-2 focus:ring-primary/30 focus:border-primary transition-all font-body-md"
            placeholder="Buscar por nombre o correo electrónico..."
            type="text"
          />
        </div>

        <div class="flex items-center justify-between px-1 text-xs text-on-surface-variant">
          <label class="flex items-center gap-2 cursor-pointer select-none hover:text-on-surface transition-colors">
            <input
              type="checkbox"
              v-model="hideAlreadyShared"
              class="rounded border-outline-variant text-primary focus:ring-primary bg-surface-variant/50 w-4 h-4 accent-primary"
            />
            <span>Ocultar usuarios ya compartidos</span>
          </label>

          <span v-if="alreadySharedUserIds.size > 0" class="text-emerald-400/90 flex items-center gap-1 font-medium">
            <span class="material-symbols-outlined text-sm">group</span>
            {{ alreadySharedUserIds.size }} compartida(s)
          </span>
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

        <div
          v-for="user in filteredUsers"
          :key="user.id"
          class="flex items-center gap-4 p-3 rounded-xl transition-colors group select-none"
          :class="[
            alreadySharedUserIds.has(user.id)
              ? 'opacity-60 bg-surface-variant/20 cursor-not-allowed'
              : 'hover:bg-surface-variant/50 cursor-pointer'
          ]"
          @click="toggleUser(user.id)"
        >
          <div class="relative flex items-center">
            <!-- Checkbox visual si ya está compartida -->
            <div
              v-if="alreadySharedUserIds.has(user.id)"
              class="w-5 h-5 border-2 border-emerald-500/50 bg-emerald-500/20 text-emerald-400 rounded flex items-center justify-center"
              title="Esta reseña ya fue compartida con este usuario"
            >
              <span class="material-symbols-outlined text-[16px]">check</span>
            </div>

            <!-- Checkbox normal -->
            <div
              v-else
              class="w-5 h-5 border-2 border-outline-variant rounded transition-colors flex items-center justify-center"
              :class="selectedUsers.includes(user.id) ? 'bg-primary border-primary' : ''"
            >
              <span
                class="material-symbols-outlined text-[16px] text-on-primary transition-all duration-200"
                :class="selectedUsers.includes(user.id) ? 'opacity-100 scale-100' : 'opacity-0 scale-50'"
              >
                check
              </span>
            </div>
          </div>

          <div
            class="w-10 h-10 rounded-full flex items-center justify-center font-bold text-sm"
            :class="alreadySharedUserIds.has(user.id) ? 'bg-surface-variant text-on-surface-variant' : 'bg-primary/10 text-primary'"
          >
            {{ getUserInitials(user.nombre) }}
          </div>

          <div class="flex-1 min-w-0">
            <div class="flex items-center gap-2">
              <span
                class="font-label-md text-sm truncate font-semibold"
                :class="alreadySharedUserIds.has(user.id) ? 'text-on-surface/80' : 'text-on-surface group-hover:text-primary transition-colors'"
              >
                {{ user.nombre }}
              </span>
              <span
                v-if="alreadySharedUserIds.has(user.id)"
                class="inline-flex items-center gap-1 text-[11px] px-2 py-0.5 rounded-full bg-emerald-500/10 text-emerald-400 border border-emerald-500/20 font-medium shrink-0"
              >
                <span class="material-symbols-outlined text-[13px]">done_all</span>
                Ya compartida
              </span>
            </div>
            <div class="font-caption text-caption text-on-surface-variant truncate">
              {{ user.correo }}
            </div>
          </div>

          <span class="text-xs px-2 py-0.5 rounded bg-surface-variant text-on-surface-variant font-mono uppercase shrink-0">
            {{ user.rol }}
          </span>
        </div>
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