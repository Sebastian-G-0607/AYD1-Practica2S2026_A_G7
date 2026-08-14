<script setup lang="ts">
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'

interface Props {
  isOpen?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  isOpen: false,
})

const emit = defineEmits<{
  (e: 'close'): void
}>()

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

interface NavItem {
  name: string
  label: string
  to: string
  icon: string
  isImplemented?: boolean
}

// Opciones del Menú para usuario Estándar
const standardNavItems: NavItem[] = [
  {
    name: 'Home',
    label: 'Tablero',
    to: '/home',
    icon: 'dashboard',
    isImplemented: true,
  },
  {
    name: 'Destacados',
    label: 'Destacados',
    to: '#',
    icon: 'star',
    isImplemented: false,
  },
  {
    name: 'Archivados',
    label: 'Archivados',
    to: '/archivadas',
    icon: 'archive',
    isImplemented: true,
  },
  {
    name: 'CompartidosConmigo',
    label: 'Compartidos Conmigo',
    to: '#',
    icon: 'group',
    isImplemented: false,
  },
  {
    name: 'MisCompartidos',
    label: 'Mis Compartidos',
    to: '#',
    icon: 'share',
    isImplemented: false,
  },
]

// Opciones del Menú Principal para usuario Admin
const adminMainNavItems: NavItem[] = [
  {
    name: 'AdminPanel',
    label: 'Panel',
    to: '/admin',
    icon: 'dashboard',
    isImplemented: true,
  },
]

// Opciones de Administración para usuario Admin (Stitch)
const adminSectionNavItems: NavItem[] = [
  {
    name: 'PendingRequests',
    label: 'Solicitudes Pendientes',
    to: '#',
    icon: 'pending_actions',
    isImplemented: false,
  },
  {
    name: 'RequestHistory',
    label: 'Historial de Solicitudes',
    to: '#',
    icon: 'history',
    isImplemented: false,
  },
  {
    name: 'AdminReports',
    label: 'Reportes de Admin',
    to: '/admin/reportes',
    icon: 'analytics',
    isImplemented: true,
  },
]

function isCurrentRoute(item: NavItem) {
  if (!item.isImplemented || item.to === '#') return false
  return route.path === item.to
}

function handleLogout() {
  authStore.logout()
  router.push({ name: 'Login' })
}

function closeMobileSidebar() {
  emit('close')
}
</script>

<template>
  <!-- Overlay para móviles -->
  <div
    v-if="props.isOpen"
    class="fixed inset-0 z-40 bg-black/70 backdrop-blur-sm lg:hidden transition-opacity"
    @click="closeMobileSidebar"
  />

  <!-- Contenedor Lateral (Sidebar) estilo Stitch Reutilizable -->
  <aside
    :class="[
      'fixed top-0 bottom-0 left-0 z-50 w-72 bg-surface-container-lowest border-r border-outline-variant/10 shadow-2xl flex flex-col justify-between transition-transform duration-300 ease-in-out lg:translate-x-0',
      props.isOpen ? 'translate-x-0' : '-translate-x-full lg:translate-x-0'
    ]"
  >
    <!-- Encabezado / Logo Stitch -->
    <div class="px-8 py-8 border-b border-outline-variant/10 flex items-center justify-between">
      <div class="flex items-center gap-3">
        <span class="material-symbols-outlined text-primary text-[32px]">movie</span>
        <div>
          <span class="font-headline-md text-headline-md tracking-tight text-on-surface uppercase font-bold">
            CineCraft
          </span>
          <p class="text-xs text-primary font-medium">
            {{ authStore.isAdmin ? 'Panel Admin' : 'Panel Estándar' }}
          </p>
        </div>
      </div>

      <!-- Botón cerrar en móvil -->
      <button
        type="button"
        class="lg:hidden text-on-surface-variant hover:text-on-surface p-1"
        @click="closeMobileSidebar"
      >
        <span class="material-symbols-outlined">close</span>
      </button>
    </div>

    <!-- Navegación Reutilizable según Rol (Stitch) -->
    <div class="flex-1 overflow-y-auto px-4 py-6 space-y-6">
      
      <!-- ==================================== -->
      <!-- MENU PARA USUARIO ADMIN             -->
      <!-- ==================================== -->
      <template v-if="authStore.isAdmin">
        <!-- Menú Principal Admin -->
        <div>
          <p class="px-4 py-2 text-on-surface-variant font-label-md text-label-md uppercase tracking-widest opacity-50 mb-1">
            Menú Principal
          </p>

          <nav class="space-y-2">
            <template v-for="item in adminMainNavItems" :key="item.name">
              <router-link
                v-if="item.isImplemented"
                :to="item.to"
                class="flex items-center px-4 py-3 rounded-lg font-medium text-sm transition-all group"
                :class="[
                  isCurrentRoute(item)
                    ? 'bg-primary-container text-on-primary-container shadow-lg shadow-primary-container/20 font-semibold'
                    : 'text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface'
                ]"
                @click="closeMobileSidebar"
              >
                <span class="material-symbols-outlined mr-4 group-hover:scale-110 transition-transform">
                  {{ item.icon }}
                </span>
                <span>{{ item.label }}</span>
              </router-link>

              <a
                v-else
                href="#"
                class="flex items-center px-4 py-3 rounded-lg font-medium text-sm transition-all group text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface"
                @click.prevent="closeMobileSidebar"
              >
                <span class="material-symbols-outlined mr-4 group-hover:scale-110 transition-transform">
                  {{ item.icon }}
                </span>
                <span>{{ item.label }}</span>
              </a>
            </template>
          </nav>
        </div>

        <!-- Sección Administración Stitch -->
        <div>
          <p class="px-4 py-2 text-on-surface-variant font-label-md text-label-md uppercase tracking-widest opacity-50 mb-1">
            Administración
          </p>

          <nav class="space-y-2">
            <template v-for="item in adminSectionNavItems" :key="item.name">
              <router-link
                v-if="item.isImplemented"
                :to="item.to"
                class="flex items-center px-4 py-3 rounded-lg font-medium text-sm transition-all group"
                :class="[
                  isCurrentRoute(item)
                    ? 'bg-primary-container text-on-primary-container shadow-lg shadow-primary-container/20 font-semibold'
                    : 'text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface'
                ]"
                @click="closeMobileSidebar"
              >
                <span class="material-symbols-outlined mr-4 group-hover:scale-110 transition-transform">
                  {{ item.icon }}
                </span>
                <span>{{ item.label }}</span>
              </router-link>

              <a
                v-else
                href="#"
                class="flex items-center px-4 py-3 rounded-lg font-medium text-sm transition-all group text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface"
                @click.prevent="closeMobileSidebar"
              >
                <span class="material-symbols-outlined mr-4 group-hover:scale-110 transition-transform">
                  {{ item.icon }}
                </span>
                <span>{{ item.label }}</span>
              </a>
            </template>
          </nav>
        </div>
      </template>

      <!-- ==================================== -->
      <!-- MENU PARA USUARIO ESTÁNDAR          -->
      <!-- ==================================== -->
      <template v-else>
        <div>
          <p class="px-4 py-2 text-on-surface-variant font-label-md text-label-md uppercase tracking-widest opacity-50 mb-1">
            Menú Principal
          </p>

          <nav class="space-y-2">
            <template v-for="item in standardNavItems" :key="item.name">
              <router-link
                v-if="item.isImplemented"
                :to="item.to"
                class="flex items-center px-4 py-3 rounded-lg font-medium text-sm transition-all group"
                :class="[
                  isCurrentRoute(item)
                    ? 'bg-primary-container text-on-primary-container shadow-lg shadow-primary-container/20 font-semibold'
                    : 'text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface'
                ]"
                @click="closeMobileSidebar"
              >
                <span class="material-symbols-outlined mr-4 group-hover:scale-110 transition-transform">
                  {{ item.icon }}
                </span>
                <span>{{ item.label }}</span>
              </router-link>

              <a
                v-else
                href="#"
                class="flex items-center px-4 py-3 rounded-lg font-medium text-sm transition-all group text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface"
                @click.prevent="closeMobileSidebar"
              >
                <span class="material-symbols-outlined mr-4 group-hover:scale-110 transition-transform">
                  {{ item.icon }}
                </span>
                <span>{{ item.label }}</span>
              </a>
            </template>
          </nav>
        </div>
      </template>

    </div>

    <!-- Enlace Configuración de Perfil (Stitch) -->
    <div class="px-4 py-3 border-t border-outline-variant/10">
      <a
        href="#"
        class="flex items-center px-4 py-3 rounded-lg text-sm font-medium transition-all group text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface"
        @click.prevent="closeMobileSidebar"
      >
        <span class="material-symbols-outlined mr-4 group-hover:scale-110 transition-transform">person</span>
        <span>Configuración de Perfil</span>
      </a>
    </div>

    <!-- Perfil del usuario y Cerrar sesión -->
    <div class="px-4 py-5 border-t border-outline-variant/10 bg-surface-container-lowest">
      <div class="flex items-center justify-between gap-3">
        <div class="flex items-center gap-3 overflow-hidden">
          <div class="w-10 h-10 rounded-full bg-surface-container-high flex items-center justify-center text-primary font-bold text-sm shrink-0 border border-outline-variant/20">
            <span class="material-symbols-outlined text-primary">person</span>
          </div>
          <div class="truncate min-w-0">
            <p class="text-sm font-semibold text-on-surface truncate">
              {{ authStore.user?.nombre || 'Usuario' }}
            </p>
            <p class="text-xs text-on-surface-variant/70 truncate">
              {{ authStore.user?.email || 'admin@cinecraft.com' }}
            </p>
            <span class="inline-block mt-0.5 px-2 py-0.5 rounded text-[10px] font-bold bg-surface-container-high text-primary uppercase tracking-wider">
              ROL: {{ authStore.user?.role || 'estandar' }}
            </span>
          </div>
        </div>

        <button
          type="button"
          class="p-2 text-on-surface-variant hover:text-red-400 hover:bg-red-500/10 rounded-lg transition-colors shrink-0"
          title="Cerrar sesión"
          @click="handleLogout"
        >
          <span class="material-symbols-outlined">logout</span>
        </button>
      </div>
    </div>
  </aside>
</template>
