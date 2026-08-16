<script setup lang="ts">
import { onMounted } from 'vue'
import { useShares } from '@/composables/useShares'

const { sharedWithMe, isLoading, error, fetchSharedWithMe } = useShares()

const getInitials = (name?: string) => {
  if (!name) return 'U'
  const parts = name.trim().split(/\s+/)
  const first = parts[0]
  const second = parts[1]
  if (first && second && first.length > 0 && second.length > 0) {
    return (first.charAt(0) + second.charAt(0)).toUpperCase()
  }
  return name.substring(0, 2).toUpperCase() || 'U'
}

onMounted(() => {
  fetchSharedWithMe()
})
</script>

<template>
  <div class="space-y-8 max-w-7xl mx-auto">
    <!-- Encabezado de Sección -->
    <div class="flex flex-col md:flex-row items-start md:items-center justify-between gap-4">
      <div>
        <h1 class="font-display-lg text-3xl md:text-4xl font-bold text-on-surface mb-2">
          Compartidos Conmigo
        </h1>
        <p class="font-body-lg text-on-surface-variant max-w-2xl text-sm md:text-base">
          Reseñas y recomendaciones cinematográficas enviadas por otros cinéfilos de la plataforma.
        </p>
      </div>

      <div
        class="hidden sm:flex items-center gap-3 bg-surface-container px-5 py-3 rounded-2xl border border-outline-variant/10 shadow-lg shrink-0"
      >
        <span class="material-symbols-outlined text-primary text-2xl">mark_email_unread</span>
        <div>
          <span class="font-label-md text-[10px] text-on-surface-variant uppercase tracking-widest block opacity-70">
            Reseñas Recibidas
          </span>
          <span class="font-headline-md text-lg font-bold text-on-surface">
            {{ sharedWithMe.length }}
          </span>
        </div>
      </div>
    </div>

    <!-- Estado de Carga -->
    <div v-if="isLoading" class="p-16 text-center text-on-surface-variant flex flex-col items-center gap-4">
      <span class="material-symbols-outlined text-4xl animate-spin text-primary">sync</span>
      <p>Cargando reseñas compartidas contigo...</p>
    </div>

    <!-- Estado de Error -->
    <div
      v-else-if="error"
      class="p-6 rounded-xl bg-red-500/10 border border-red-500/20 text-red-300 flex items-center gap-3"
    >
      <span class="material-symbols-outlined text-2xl">error</span>
      <p class="text-sm font-medium">{{ error }}</p>
    </div>

    <!-- Estado Vacío -->
    <div
      v-else-if="sharedWithMe.length === 0"
      class="bg-surface-container rounded-2xl p-16 text-center text-on-surface-variant border border-outline-variant/10 shadow-xl flex flex-col items-center gap-4 my-6"
    >
      <span class="material-symbols-outlined text-6xl text-on-surface-variant opacity-40">mark_email_read</span>
      <h3 class="text-xl font-bold text-on-surface">No tienes reseñas compartidas aún</h3>
      <p class="max-w-md text-sm opacity-80">
        Cuando otros usuarios compartan sus recomendaciones de películas contigo, aparecerán directamente aquí.
      </p>
    </div>

    <!-- Grid de Reseñas Compartidas -->
    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <article
        v-for="share in sharedWithMe"
        :key="`${share.reseniaId}-${share.usuarioRemitenteId}`"
        class="bg-surface-container rounded-2xl p-6 shadow-xl hover:shadow-2xl transition-all duration-300 relative overflow-hidden border border-outline-variant/10 flex flex-col justify-between group hover:-translate-y-1"
      >
        <!-- Cabecera de la tarjeta: Badge e Indicador de Compartido -->
        <div class="space-y-4">
          <div class="flex items-center justify-between">
            <span
              class="flex items-center gap-1.5 text-primary bg-primary/10 px-3 py-1 rounded-full text-xs font-semibold border border-primary/20"
            >
              <span class="material-symbols-outlined text-sm">share</span>
              Compartida
            </span>

            <!-- Calificación con estrellas -->
            <div class="flex items-center gap-0.5 text-yellow-400 text-sm">
              <span>{{ '★'.repeat(share.calificacion || 5) }}</span>
              <span class="text-on-surface/20">{{ '★'.repeat(5 - (share.calificacion || 5)) }}</span>
            </div>
          </div>

          <!-- Título y Etiqueta -->
          <div>
            <h3 class="font-headline-md text-xl font-bold text-on-surface group-hover:text-primary transition-colors line-clamp-1">
              {{ share.tituloPelicula }}
            </h3>

            <span
              v-if="share.etiqueta"
              class="inline-block mt-2 text-xs px-2.5 py-0.5 rounded-full bg-surface-container-high text-on-surface-variant font-medium border border-outline-variant/10"
            >
              {{ share.etiqueta }}
            </span>
          </div>

          <!-- Comentario -->
          <p class="font-body-md text-sm text-on-surface-variant opacity-85 line-clamp-4 leading-relaxed italic bg-surface-dim/30 p-3 rounded-xl border border-outline-variant/5">
            "{{ share.comentario || 'Sin reseña escrita.' }}"
          </p>
        </div>

        <!-- Información de Remitente y Autor Original -->
        <div class="pt-5 border-t border-outline-variant/10 space-y-2 mt-6">
          <div class="flex items-center gap-3">
            <div
              class="w-10 h-10 rounded-full bg-primary/20 text-primary flex items-center justify-center font-bold text-sm shadow-sm shrink-0"
            >
              {{ getInitials(share.remitenteNombre) }}
            </div>
            <div class="min-w-0 flex-1">
              <p class="text-[10px] text-primary uppercase font-bold tracking-wider">
                Compartido por
              </p>
              <p class="font-label-md text-sm font-semibold text-on-surface truncate">
                {{ share.remitenteNombre }}
              </p>
              <p class="text-xs text-on-surface-variant opacity-60 truncate">
                {{ share.remitenteCorreo }}
              </p>
            </div>
          </div>

          <div
            v-if="share.autorNombre && share.autorNombre !== share.remitenteNombre"
            class="text-[11px] text-on-surface-variant/70 pl-13 pt-1"
          >
            Autor original: <span class="font-semibold text-on-surface">{{ share.autorNombre }}</span>
          </div>
        </div>
      </article>
    </div>
  </div>
</template>