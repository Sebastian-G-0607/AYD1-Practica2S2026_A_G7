<script setup lang="ts">
import BaseInput from '@/components/common/BaseInput.vue'
import BaseButton from '@/components/common/BaseButton.vue'
import { useRegister } from '@/composables/useRegister'

const {
  form,
  formErrors,
  isLoading,
  apiError,
  isSuccess,
  successData,
  handleRegister,
} = useRegister()
</script>

<template>
  <div class="w-full flex flex-col items-center animate-fade-in-up">
    <div class="flex flex-col items-center text-center mb-8">
      <div
        class="w-16 h-16 rounded-full bg-primary-container/20 border border-primary-container/40 flex items-center justify-center mb-4 shadow-lg shadow-primary-container/10"
      >
        <span class="material-symbols-outlined text-3xl text-primary-container">
          person_add
        </span>
      </div>
      <h1 class="text-3xl md:text-4xl font-extrabold tracking-tight text-on-surface font-headline-lg">
        CineCraft
      </h1>
      <p class="text-sm text-on-surface/60 font-body-md mt-1.5">
        Solicitud de Registro
      </p>
    </div>

    <div
      class="w-full bg-surface-container-low/70 backdrop-blur-xl border border-surface-bright/50 rounded-2xl p-6 sm:p-8 shadow-2xl shadow-black/50"
    >
      <div v-if="isSuccess" class="flex flex-col items-center text-center py-4 animate-fade-in-up">
        <div class="w-16 h-16 rounded-full bg-emerald-500/20 border border-emerald-500/40 flex items-center justify-center mb-4 text-emerald-400">
          <span class="material-symbols-outlined text-4xl">
            check_circle
          </span>
        </div>

        <h2 class="text-2xl font-bold text-on-surface font-headline-lg mb-2">
          ¡Solicitud enviada!
        </h2>

        <p class="text-sm text-on-surface/70 font-body-md mb-6 max-w-sm">
          Tu solicitud de registro ha sido enviada exitosamente. Recibirás una respuesta pronto cuando un administrador evalúe tu cuenta.
        </p>

        <div v-if="successData" class="w-full bg-surface-dim/80 border border-surface-bright/60 rounded-xl p-4 mb-6 text-left space-y-2 text-xs font-body-md">
          <div class="flex justify-between text-on-surface/70">
            <span>ID Solicitud:</span>
            <span class="font-mono font-semibold text-on-surface">#{{ successData.id }}</span>
          </div>
          <div class="flex justify-between text-on-surface/70">
            <span>Nombre:</span>
            <span class="font-semibold text-on-surface">{{ successData.nombre }}</span>
          </div>
          <div class="flex justify-between text-on-surface/70">
            <span>Correo:</span>
            <span class="font-semibold text-on-surface">{{ successData.correo }}</span>
          </div>
          <div class="flex justify-between text-on-surface/70">
            <span>Estado:</span>
            <span class="px-2 py-0.5 rounded-full bg-amber-500/20 text-amber-300 font-semibold text-[11px]">
              {{ successData.status || 'Pendiente' }}
            </span>
          </div>
        </div>

        <router-link :to="{ name: 'Login' }" class="w-full">
          <BaseButton
            type="button"
            label="Volver al Iniciar Sesión"
            icon="login"
          />
        </router-link>
      </div>

      <form v-else @submit.prevent="handleRegister" class="space-y-5" novalidate>
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
            id="register-nombre"
            v-model="form.nombre"
            type="text"
            label="Nombre Completo"
            placeholder="Tu nombre completo"
            icon="person"
            :error="formErrors.nombre"
            :disabled="isLoading"
          />

          <BaseInput
            id="register-correo"
            v-model="form.correo"
            type="email"
            label="Correo Electrónico"
            placeholder="cinefilo@cinecraft.com"
            icon="mail"
            :error="formErrors.correo"
            :disabled="isLoading"
          />

          <BaseInput
            id="register-contrasenia"
            v-model="form.contrasenia"
            type="password"
            label="Contraseña"
            placeholder="••••••••"
            icon="lock"
            :error="formErrors.contrasenia"
            :disabled="isLoading"
          />

          <BaseInput
            id="register-confirmacion"
            v-model="form.confirmacion_contrasenia"
            type="password"
            label="Confirmar Contraseña"
            placeholder="••••••••"
            icon="lock"
            :error="formErrors.confirmacion_contrasenia"
            :disabled="isLoading"
          />
        </div>

        <div class="pt-2">
          <BaseButton
            type="submit"
            label="Solicitar Registro"
            loading-label="Procesando..."
            icon="app_registration"
            :loading="isLoading"
          />
        </div>

        <div class="mt-6 pt-6 border-t border-surface-bright/40 text-center">
          <p class="text-xs text-on-surface/60 font-body-md">
            ¿Ya tienes una cuenta?
            <router-link
              :to="{ name: 'Login' }"
              class="text-primary hover:text-primary/80 font-semibold underline underline-offset-4 ml-1 transition-colors"
            >
              Iniciar Sesión
            </router-link>
          </p>
        </div>
      </form>
    </div>
  </div>
</template>
