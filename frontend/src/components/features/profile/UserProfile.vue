<template>
  <aside
    class="fixed left-0 top-0 h-full w-72 bg-surface-container-lowest z-50 flex flex-col border-r border-outline-variant/10 shadow-2xl">
    <div class="px-8 py-10 flex items-center gap-3"><span
        class="material-symbols-outlined text-primary text-3xl">movie</span><span
        class="font-headline-md text-headline-md tracking-tight text-on-surface uppercase">CineCraft</span></div>
    <nav class="flex-1 px-4 space-y-2 overflow-y-auto"
      data-active-classes="bg-primary-container text-on-primary-container shadow-lg shadow-primary-container/20">
      <div class="px-4 py-2 text-on-surface-variant font-label-md text-label-md uppercase tracking-widest opacity-50">
        Menú Principal</div><a
        class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
        data-path="dashboard" href="#"><span
          class="material-symbols-outlined mr-4 group-hover:text-primary">dashboard</span>Panel</a><a
        class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
        data-path="highlights" href="#"><span
          class="material-symbols-outlined mr-4 group-hover:text-secondary-container">star</span>Destacados</a><a
        class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
        data-path="archived" href="#"><span
          class="material-symbols-outlined mr-4 group-hover:text-on-surface">archive</span>Archivado</a><a
        class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
        data-path="shared-with-me" href="#"><span
          class="material-symbols-outlined mr-4 group-hover:text-on-surface">group</span>Compartido Conmigo</a><a
        class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
        data-path="my-shared" href="#"><span
          class="material-symbols-outlined mr-4 group-hover:text-on-surface">share</span>Mis Compartidos</a>
      <div
        class="pt-8 px-4 py-2 text-on-surface-variant font-label-md text-label-md uppercase tracking-widest opacity-50">
        Administración</div><a
        class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
        data-path="pending-requests" href="#"><span
          class="material-symbols-outlined mr-4 group-hover:text-primary">pending_actions</span>Solicitudes
        Pendientes</a><a
        class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
        data-path="request-history" href="#"><span
          class="material-symbols-outlined mr-4 group-hover:text-on-surface">history</span>Historial de
        Solicitudes</a><a
        class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
        data-path="admin-reports" href="#"><span
          class="material-symbols-outlined mr-4 group-hover:text-on-surface">analytics</span>Reportes de
        Administrador</a>
    </nav>
    <div class="px-4 py-6 border-t border-outline-variant/10"><a
        class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
        data-path="profile" href="#"><span
          class="material-symbols-outlined mr-4 group-hover:text-on-surface">person</span>Configuración de Perfil</a>
    </div>
  </aside>
  <div class="pl-72 min-h-screen flex flex-col">
    <header
      class="fixed top-0 left-72 right-0 h-20 bg-surface/80 backdrop-blur-2xl z-40 px-8 flex items-center justify-between border-b border-outline-variant/5 shadow-sm">
      <div class="flex-1 max-w-2xl relative group"><span
          class="material-symbols-outlined absolute left-4 top-1/2 -translate-y-1/2 text-on-surface-variant group-focus-within:text-primary transition-colors">search</span><input
          class="w-full bg-surface-container-high/50 border border-outline-variant/10 rounded-full py-2.5 pl-12 pr-6 text-on-surface placeholder:text-on-surface-variant/50 focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all font-body-md"
          placeholder="Buscar películas, reseñas, o creadores..." type="text" /></div>
      <div class="flex items-center gap-6"><button
          class="relative p-2 rounded-full hover:bg-surface-variant transition-colors text-on-surface-variant hover:text-on-surface"><span
            class="material-symbols-outlined">notifications</span><span
            class="absolute top-2 right-2 w-2 h-2 bg-primary rounded-full"></span></button>
        <div class="flex items-center gap-3 pl-4 border-l border-outline-variant/20 hover:cursor-pointer group">
          <div class="text-right hidden sm:block">
            <div class="text-label-md font-label-md text-on-surface">Alex Rivera</div>
            <div class="text-caption font-caption text-on-surface-variant opacity-70">Crítico Pro</div>
          </div>
          <div
            class="w-10 h-10 rounded-full bg-primary/20 text-primary flex items-center justify-center font-label-md text-label-md ring-2 ring-transparent group-hover:ring-primary transition-all">
            AR</div>
        </div>
      </div>
    </header>
    <main class="flex-1 pt-20 bg-surface p-8">
      <div class="flex flex-col w-full max-w-4xl mx-auto gap-8">
        <div class="flex flex-col gap-2">
          <h1 class="font-display-lg text-display-lg text-on-surface">Editar Perfil</h1>
          <p class="font-body-lg text-body-lg text-on-surface-variant opacity-80">Administra tu identidad
            cinematográfica y preferencias.</p>
        </div>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
          <div class="md:col-span-1 flex flex-col gap-6">
            <div
              class="bg-surface-container rounded-xl p-6 flex flex-col items-center gap-4 shadow-lg border border-outline-variant/10">
              <div class="relative group cursor-pointer">
                <div
                  class="w-40 h-40 rounded-full bg-primary/20 text-primary flex items-center justify-center font-display-lg text-6xl shadow-2xl transition-transform duration-300 group-hover:scale-105">
                  AR</div>
                <div
                  class="absolute inset-0 bg-background/60 rounded-full opacity-0 group-hover:opacity-100 transition-opacity duration-300 flex items-center justify-center backdrop-blur-sm">
                  <span class="material-symbols-outlined text-on-surface text-3xl">photo_camera</span>
                </div>
              </div>
              <div class="text-center">
                <h2 class="font-headline-md text-headline-md text-on-surface">Alex Rivera</h2>
                <p
                  class="font-label-md text-label-md text-on-surface-variant uppercase tracking-widest mt-1 opacity-70">
                  Crítico Pro</p>
              </div>
              <button
                class="w-full mt-2 py-2 px-4 rounded-lg border border-outline-variant/30 text-on-surface font-label-md text-label-md hover:bg-surface-container-high transition-colors">
                Subir Nueva Imagen
              </button>
            </div>
            <div
              class="bg-surface-container rounded-xl p-6 flex flex-col gap-4 shadow-lg border border-outline-variant/10">
              <h3 class="font-headline-md text-headline-md text-on-surface flex items-center gap-2">
                <span class="material-symbols-outlined text-primary">local_movies</span>
                Estadísticas
              </h3>
              <div class="grid grid-cols-2 gap-4">
                <div class="flex flex-col">
                  <span class="font-display-lg text-display-lg text-primary">142</span>
                  <span class="font-label-md text-label-md text-on-surface-variant uppercase opacity-70">Reseñas</span>
                </div>
                <div class="flex flex-col">
                  <span class="font-display-lg text-display-lg text-secondary-container">89</span>
                  <span class="font-label-md text-label-md text-on-surface-variant uppercase opacity-70">Listas</span>
                </div>
              </div>
            </div>
          </div>
          <div class="md:col-span-2 flex flex-col gap-6">
            <div class="bg-surface-container rounded-xl p-8 shadow-lg border border-outline-variant/10">
              <h3
                class="font-headline-md text-headline-md text-on-surface mb-6 border-b border-outline-variant/20 pb-4">
                Detalles Personales</h3>
              <form class="flex flex-col gap-6">
                <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div class="flex flex-col gap-2">
                    <label class="font-label-md text-label-md text-on-surface-variant">Nombre Completo</label>
                    <input
                      class="bg-surface-dim border border-outline-variant/20 rounded-lg px-4 py-3 text-on-surface font-body-md focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary transition-colors"
                      type="text" value="Alex Rivera" />
                  </div>
                  <div class="flex flex-col gap-2">
                    <label class="font-label-md text-label-md text-on-surface-variant">Nombre de Usuario</label>
                    <input
                      class="bg-surface-dim border border-outline-variant/20 rounded-lg px-4 py-3 text-on-surface font-body-md focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary transition-colors"
                      type="text" value="@arivera_cine" />
                  </div>
                </div>
                <div class="flex flex-col gap-2">
                  <label class="font-label-md text-label-md text-on-surface-variant">Correo Electrónico</label>
                  <input
                    class="bg-surface-dim border border-outline-variant/20 rounded-lg px-4 py-3 text-on-surface font-body-md focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary transition-colors"
                    type="email" value="alex.rivera@cinecraft.app" />
                </div>
                <h3
                  class="font-headline-md text-headline-md text-on-surface mt-4 mb-2 border-b border-outline-variant/20 pb-4">
                  Seguridad</h3>
                <div class="flex flex-col gap-2">
                  <label class="font-label-md text-label-md text-on-surface-variant">Contraseña Actual</label>
                  <input
                    class="bg-surface-dim border border-outline-variant/20 rounded-lg px-4 py-3 text-on-surface font-body-md focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary transition-colors"
                    placeholder="••••••••" type="password" />
                </div>
                <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div class="flex flex-col gap-2">
                    <label class="font-label-md text-label-md text-on-surface-variant">Nueva Contraseña</label>
                    <input
                      class="bg-surface-dim border border-outline-variant/20 rounded-lg px-4 py-3 text-on-surface font-body-md focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary transition-colors"
                      type="password" />
                  </div>
                  <div class="flex flex-col gap-2">
                    <label class="font-label-md text-label-md text-on-surface-variant">Confirmar Nueva
                      Contraseña</label>
                    <input
                      class="bg-surface-dim border border-outline-variant/20 rounded-lg px-4 py-3 text-on-surface font-body-md focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary transition-colors"
                      type="password" />
                  </div>
                </div>
              </form>
            </div>
            <div class="bg-surface-container rounded-xl p-8 shadow-lg border border-outline-variant/10">
              <h3 class="font-headline-md text-headline-md text-on-surface mb-2">Géneros Favoritos</h3>
              <p class="font-body-md text-body-md text-on-surface-variant opacity-70 mb-6">Selecciona los géneros que
                definen tu gusto cinematográfico para mejorar las recomendaciones.</p>
              <div class="flex flex-wrap gap-3">
                <button
                  class="px-4 py-2 rounded-full border border-primary bg-primary/10 text-primary font-label-md text-label-md transition-colors hover:bg-primary/20">Ciencia
                  Ficción</button>
                <button
                  class="px-4 py-2 rounded-full border border-primary bg-primary/10 text-primary font-label-md text-label-md transition-colors hover:bg-primary/20">Neo-Noir</button>
                <button
                  class="px-4 py-2 rounded-full border border-outline-variant/30 text-on-surface-variant font-label-md text-label-md transition-colors hover:border-on-surface hover:text-on-surface">Suspense</button>
                <button
                  class="px-4 py-2 rounded-full border border-primary bg-primary/10 text-primary font-label-md text-label-md transition-colors hover:bg-primary/20">Cine
                  de Autor</button>
                <button
                  class="px-4 py-2 rounded-full border border-outline-variant/30 text-on-surface-variant font-label-md text-label-md transition-colors hover:border-on-surface hover:text-on-surface">Terror</button>
                <button
                  class="px-4 py-2 rounded-full border border-outline-variant/30 text-on-surface-variant font-label-md text-label-md transition-colors hover:border-on-surface hover:text-on-surface">Documental</button>
                <button
                  class="px-4 py-2 rounded-full border border-outline-variant/30 text-on-surface-variant font-label-md text-label-md transition-colors hover:border-on-surface hover:text-on-surface">Acción</button>
              </div>
            </div>
            <div class="flex justify-end pt-4 pb-12">
              <button
                class="bg-primary hover:bg-primary-container text-on-primary font-label-md text-label-md px-8 py-4 rounded-lg shadow-lg shadow-primary/20 transition-all duration-300 transform hover:-translate-y-1">
                Guardar Cambios
              </button>
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>