import { createRouter, createWebHistory } from 'vue-router'
import type { RouteRecordRaw } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'Root',
    redirect: () => {
      const authStore = useAuthStore()
      if (authStore.isAuthenticated) {
        return authStore.isAdmin ? { name: 'Admin' } : { name: 'Home' }
      }
      return { name: 'Login' }
    },
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/views/LoginView.vue'),
    meta: { requiresAuth: false },
  },
  {
    path: '/register-request',
    name: 'RegisterRequest',
    component: () => import('@/views/RegisterView.vue'),
    meta: { requiresAuth: false },
  },
  {
    path: '/home',
    name: 'Home',
    component: () => import('@/views/HomeView.vue'),
    meta: { requiresAuth: true, role: 'estandar' },
  },

  {
    path: '/archivadas',
    name: 'Archivadas',
    component: () => import('@/views/ArchivadasView.vue'),
    meta: { requiresAuth: true, role: 'estandar' },
  },

    {
    path: '/destacadas',
    name: 'Destacadas',
    component: () => import('@/views/DestacadasView.vue'),
    meta: { requiresAuth: true, role: 'estandar' },
  },

  {
    path: '/admin',
    name: 'Admin',
    component: () => import('@/views/AdminHomeView.vue'),
    meta: { requiresAuth: true, role: 'admin' },
  },
  {
    path: '/admin/reportes',
    name: 'AdminReports',
    component: () => import('@/views/AdminReportsView.vue'),
    meta: { requiresAuth: true, role: 'admin' },
  },
  {
    path: '/shared-with-me',
    name: 'SharedWithMe',
    component: () => import('@/views/SharedWithMeView.vue'),
    meta: { requiresAuth: true, role: 'estandar' },
  },
  {
    path: '/my-shares',
    name: 'MyShares',
    component: () => import('@/views/MySharesView.vue'),
    meta: { requiresAuth: true, role: 'estandar' },
  },
  {
    path: '/profile',
    name: 'Profile',
    component: () => import('@/views/ProfileView.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/:pathMatch(.*)*',
    redirect: () => {
      const authStore = useAuthStore()
      if (authStore.isAuthenticated) {
        return authStore.isAdmin ? { name: 'Admin' } : { name: 'Home' }
      }
      return { name: 'Login' }
    },
  },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

router.beforeEach((to) => {
  const authStore = useAuthStore()

  // Si la ruta requiere autenticación y no se ha autenticado
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    return { name: 'Login' }
  }

  if (
    to.meta.requiresAdmin &&
    authStore.user?.role?.toLowerCase() !== 'admin'
  ) {
    return { name: 'Home' }
  }

  if (to.name === 'Login' && authStore.isAuthenticated) {
    return authStore.isAdmin ? { name: 'Admin' } : { name: 'Home' }
  }

  // Protección de roles
  if (authStore.isAuthenticated) {
    // Si la ruta es solo para admin y el usuario es estándar -> redirigir a Home
    if (to.meta.role === 'admin' && !authStore.isAdmin) {
      return { name: 'Home' }
    }
    // Si la ruta es solo para estándar y el usuario es admin -> redirigir a Admin
    if (to.meta.role === 'estandar' && authStore.isAdmin) {
      return { name: 'Admin' }
    }
  }
})

export default router
