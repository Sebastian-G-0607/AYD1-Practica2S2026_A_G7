<script setup lang="ts">
import BaseInput from '@/components/common/BaseInput.vue'
import BaseButton from '@/components/common/BaseButton.vue'
import { useAuth } from '@/composables/useAuth'

const { form, formErrors, isLoading, apiError, handleLogin } = useAuth()
</script>

<template>
  <div class="w-full flex flex-col items-center animate-fade-in-up">
    <div class="flex flex-col items-center text-center mb-8">
      <div
        class="w-16 h-16 rounded-full bg-primary-container/20 border border-primary-container/40 flex items-center justify-center mb-4 shadow-lg shadow-primary-container/10"
      >
        <span class="material-symbols-outlined text-3xl text-primary-container">
          movie
        </span>
      </div>
      <h1 class="text-3xl md:text-4xl font-extrabold tracking-tight text-on-surface font-headline-lg">
        CineCraft
      </h1>
      <p class="text-sm text-on-surface/60 font-body-md mt-1.5">
        El corte del director de tu vida cinéfila.
      </p>
    </div>

    <div
      class="w-full bg-surface-container-low/70 backdrop-blur-xl border border-surface-bright/50 rounded-2xl p-6 sm:p-8 shadow-2xl shadow-black/50"
    >
      <form @submit.prevent="handleLogin" class="space-y-6" novalidate>
        <div
          v-if="apiError"
          class="p-4 rounded-lg bg-error-container/40 border border-error/50 text-on-error-container text-sm flex items-start gap-3 animate-fade-in-up"
        >
          <span class="material-symbols-outlined text-error text-xl shrink-0 mt-0.5">
            error
          </span>
          <div class="flex-1 font-body-md">
            {{ apiError }}
          </div>
        </div>

        <div class="space-y-4">
          <BaseInput
            id="login-email"
            v-model="form.email"
            type="email"
            label="Correo Electrónico"
            placeholder="cinefilo@cinecraft.com"
            icon="mail"
            :error="formErrors.email"
            :disabled="isLoading"
          />

          <BaseInput
            id="login-password"
            v-model="form.password"
            type="password"
            label="Contraseña"
            placeholder="••••••••"
            icon="lock"
            :error="formErrors.password"
            :disabled="isLoading"
          />
        </div>

        <div class="pt-2">
          <BaseButton
            type="submit"
            label="Entrar"
            icon="arrow_forward"
            :loading="isLoading"
          />
        </div>
      </form>

      <div class="mt-8 pt-6 border-t border-surface-bright/40 text-center">
        <p class="text-xs text-on-surface/60 font-body-md">
          ¿Nuevo en el arte?
          <router-link
            :to="{ name: 'RegisterRequest' }"
            class="text-primary hover:text-primary/80 font-semibold underline underline-offset-4 ml-1 transition-colors"
          >
            Solicitud de registro
          </router-link>
        </p>
      </div>
    </div>
  </div>
</template>
