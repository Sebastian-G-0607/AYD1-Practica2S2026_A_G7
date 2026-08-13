<template>
        <aside
        class="fixed left-0 top-0 h-full w-72 bg-surface-container-lowest z-50 flex flex-col border-r border-outline-variant/10 shadow-2xl">
        <div class="px-8 py-10 flex items-center gap-3"><span
                class="material-symbols-outlined text-primary text-[32px]">movie</span><span
                class="font-headline-md text-headline-md tracking-tight text-on-surface uppercase">CineCraft</span>
        </div>
        <nav class="flex-1 px-4 space-y-2 overflow-y-auto"
            data-active-classes="bg-primary-container text-on-primary-container shadow-lg shadow-primary-container/20">
            <div
                class="px-4 py-2 text-on-surface-variant font-label-md text-label-md uppercase tracking-widest opacity-50">
                Menú Principal</div><a
                class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
                data-path="dashboard" href="#"><span
                    class="material-symbols-outlined mr-4 group-hover:text-primary">dashboard</span>Panel</a><a
                class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
                data-path="highlights" href="#"><span
                    class="material-symbols-outlined mr-4 group-hover:text-secondary-container">star</span>Destacados</a><a
                class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
                data-path="archived" href="#"><span
                    class="material-symbols-outlined mr-4 group-hover:text-on-surface">archive</span>Archivados</a><a
                aria-current="page"
                class="flex items-center px-4 py-3 rounded-lg transition-all group bg-primary-container text-on-primary-container shadow-lg shadow-primary-container/20"
                data-path="shared-with-me" href="#"><span
                    class="material-symbols-outlined mr-4 group-hover:text-on-surface">group</span>Compartidos
                Conmigo</a><a
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
                Admin</a>
        </nav>
        <div class="px-4 py-6 border-t border-outline-variant/10"><a
                class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
                data-path="profile" href="#"><span
                    class="material-symbols-outlined mr-4 group-hover:text-on-surface">person</span>Ajustes de
                Perfil</a></div>
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
                        class="w-10 h-10 rounded-full bg-surface-variant flex items-center justify-center ring-2 ring-transparent group-hover:ring-primary transition-all shadow-sm">
                        <span class="material-symbols-outlined text-on-surface">person</span></div>
                </div>
            </div>
        </header>
        <main class="flex-1 pt-20 bg-surface p-8">
            <div class="flex flex-col w-full h-full pb-16">
                <div class="mb-12 relative flex items-center justify-between">
                    <div class="relative z-10">
                        <h1 class="font-display-lg text-display-lg text-on-surface mb-2 tracking-tight">Archivo
                            Compartido</h1>
                        <p class="font-body-lg text-body-lg text-on-surface-variant max-w-2xl">Exploraciones y críticas
                            seleccionadas por tu red. Descubre nuevas perspectivas.</p>
                    </div>
                    <div
                        class="hidden lg:block relative w-64 h-24 overflow-hidden rounded-xl bg-surface-container shadow-2xl flex-shrink-0">
                        <div class="absolute inset-0 opacity-20" data-alt="Abstract pattern"
                            style="background-image: radial-gradient(circle at center, #af8782 1px, transparent 1px); background-size: 12px 12px;">
                        </div>
                        <div class="absolute inset-0 bg-gradient-to-r from-surface-container to-transparent"></div>
                        <div class="relative h-full flex flex-col justify-center px-6">
                            <span
                                class="font-label-md text-label-md text-on-surface-variant uppercase tracking-widest opacity-60 block mb-1">Actividad
                                de Red</span>
                            <div class="flex items-center gap-2">
                                <span class="relative flex h-3 w-3">
                                    <span
                                        class="animate-ping absolute inline-flex h-full w-full rounded-full bg-secondary-container opacity-75"></span>
                                    <span
                                        class="relative inline-flex rounded-full h-3 w-3 bg-secondary-container"></span>
                                </span>
                                <span class="font-headline-md text-headline-md text-on-surface">12 Nuevos</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="flex flex-col gap-12 lg:flex-row lg:gap-16">
                    <div class="w-full lg:w-3/4 flex flex-col gap-6">
                        <div class="flex items-center justify-between mb-2">
                            <h2 class="font-headline-md text-headline-md text-on-surface tracking-tight">Recibidos
                                Recientemente</h2>
                            <div class="flex gap-2">
                                <button
                                    class="w-10 h-10 rounded-full bg-surface-container-high text-on-surface-variant hover:bg-primary hover:text-on-primary transition-colors flex items-center justify-center shadow-md"><span
                                        class="material-symbols-outlined text-[20px]">filter_list</span></button>
                                <button
                                    class="w-10 h-10 rounded-full bg-surface-container-high text-on-surface-variant hover:bg-primary hover:text-on-primary transition-colors flex items-center justify-center shadow-md"><span
                                        class="material-symbols-outlined text-[20px]">sort</span></button>
                            </div>
                        </div>
                        <!-- Featured Shared Item -->
                        <article
                            class="group relative bg-surface-container rounded-2xl overflow-hidden shadow-xl hover:shadow-2xl transition-all duration-500 transform hover:-translate-y-1 isolate cursor-pointer">
                            <div class="absolute inset-0 z-0 bg-surface-container-high">
                                <div
                                    class="absolute inset-0 bg-gradient-to-t from-surface-container via-surface-container/90 to-transparent z-10">
                                </div>
                                <div class="w-full h-full flex items-center justify-center transition-transform duration-700 group-hover:scale-105 opacity-30"
                                    data-alt="Cinematic placeholder">
                                    <span
                                        class="material-symbols-outlined text-[200px] text-primary">movie_filter</span>
                                </div>
                            </div>
                            <div class="relative z-20 p-8 pt-48 flex flex-col justify-end h-full">
                                <div class="flex items-center justify-between mb-4">
                                    <div
                                        class="flex items-center gap-3 bg-surface-variant/80 backdrop-blur-md px-4 py-2 rounded-full shadow-lg">
                                        <div
                                            class="w-8 h-8 rounded-full bg-secondary-container text-on-secondary-container flex items-center justify-center font-label-md text-label-md shadow-sm">
                                            ER</div>
                                        <span class="font-label-md text-label-md text-on-surface">Por Elena
                                            Rostova</span>
                                    </div>
                                    <div
                                        class="bg-primary-container text-on-primary-container font-label-md text-label-md px-4 py-2 rounded-full uppercase tracking-widest shadow-lg flex items-center gap-2">
                                        <span class="material-symbols-outlined text-[16px]">fiber_new</span>
                                        No leídos
                                    </div>
                                </div>
                                <div class="flex items-start justify-between">
                                    <div class="max-w-xl">
                                        <h3
                                            class="font-headline-lg text-headline-lg text-on-surface mb-2 group-hover:text-primary transition-colors duration-300">
                                            Neon Paradox</h3>
                                        <p
                                            class="font-body-md text-body-md text-on-surface-variant opacity-80 line-clamp-2 mb-4">
                                            "Una obra maestra visual que se asfixia bajo el peso de su propia ambición.
                                            La cinematografía es impresionante, pero la coherencia narrativa se disuelve
                                            por completo en el tercer acto."</p>
                                    </div>
                                    <div
                                        class="flex flex-col items-end gap-2 bg-surface-dim/90 backdrop-blur-sm p-4 rounded-xl shadow-lg border border-outline-variant/10">
                                        <span
                                            class="font-label-md text-label-md text-on-surface-variant uppercase tracking-wider opacity-60">Veredicto</span>
                                        <div class="flex items-center gap-1 text-secondary-container">
                                            <span class="material-symbols-outlined"
                                                style="font-variation-settings: 'FILL' 1;">star</span>
                                            <span class="material-symbols-outlined"
                                                style="font-variation-settings: 'FILL' 1;">star</span>
                                            <span class="material-symbols-outlined"
                                                style="font-variation-settings: 'FILL' 1;">star</span>
                                            <span class="material-symbols-outlined"
                                                style="font-variation-settings: 'FILL' 1;">star_half</span>
                                            <span class="material-symbols-outlined opacity-30"
                                                style="font-variation-settings: 'FILL' 1;">star</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </article>
                        <!-- List Items -->
                        <div class="grid grid-cols-1 md:grid-cols-2 gap-6 mt-4">
                            <article
                                class="bg-surface-container rounded-2xl p-6 shadow-md hover:shadow-xl transition-all duration-300 relative overflow-hidden group cursor-pointer flex flex-col justify-between min-h-[240px]">
                                <div
                                    class="absolute -right-12 -top-12 w-32 h-32 bg-secondary-container/5 rounded-full blur-2xl group-hover:bg-secondary-container/10 transition-colors">
                                </div>
                                <div>
                                    <div class="flex items-center justify-between mb-6">
                                        <span
                                            class="flex items-center gap-2 text-secondary-container bg-secondary-container/10 px-3 py-1 rounded-full font-label-md text-label-md shadow-sm">
                                            <span class="material-symbols-outlined text-[16px]">share</span>
                                            Compartido
                                        </span>
                                        <div class="flex items-center gap-1 text-secondary-container">
                                            <span class="material-symbols-outlined text-[16px]"
                                                style="font-variation-settings: 'FILL' 1;">star</span>
                                            <span class="font-label-md text-label-md">5.0</span>
                                        </div>
                                    </div>
                                    <h3
                                        class="font-headline-md text-headline-md text-on-surface mb-2 group-hover:text-primary transition-colors line-clamp-1">
                                        The Silent Symphony</h3>
                                    <p
                                        class="font-body-md text-body-md text-on-surface-variant opacity-70 line-clamp-2">
                                        "Un triunfo absoluto de diseño de sonido. Cada cuadro se siente meticulosamente
                                        creado para evocar una abrumadora sensación de temor."</p>
                                </div>
                                <div
                                    class="mt-6 flex items-center justify-between border-t border-outline-variant/10 pt-4">
                                    <div class="flex items-center gap-3">
                                        <div
                                            class="w-8 h-8 rounded-full bg-tertiary-container text-on-tertiary-container flex items-center justify-center font-label-md text-label-md shadow-sm">
                                            JD
                                        </div>
                                        <div>
                                            <div class="font-label-md text-label-md text-on-surface">Julian Doss</div>
                                            <div class="font-caption text-caption text-on-surface-variant opacity-60">
                                                hace 2 días</div>
                                        </div>
                                    </div>
                                </div>
                            </article>
                            <article
                                class="bg-surface-container rounded-2xl p-6 shadow-md hover:shadow-xl transition-all duration-300 relative overflow-hidden group cursor-pointer flex flex-col justify-between min-h-[240px]">
                                <div
                                    class="absolute -right-12 -top-12 w-32 h-32 bg-primary/5 rounded-full blur-2xl group-hover:bg-primary/10 transition-colors">
                                </div>
                                <div>
                                    <div class="flex items-center justify-between mb-6">
                                        <span
                                            class="flex items-center gap-2 text-primary bg-primary/10 px-3 py-1 rounded-full font-label-md text-label-md shadow-sm">
                                            <span class="material-symbols-outlined text-[16px]">new_releases</span>
                                            Nuevo
                                        </span>
                                        <div class="flex items-center gap-1 text-secondary-container">
                                            <span class="material-symbols-outlined text-[16px]"
                                                style="font-variation-settings: 'FILL' 1;">star</span>
                                            <span class="font-label-md text-label-md">3.5</span>
                                        </div>
                                    </div>
                                    <h3
                                        class="font-headline-md text-headline-md text-on-surface mb-2 group-hover:text-primary transition-colors line-clamp-1">
                                        Echoes of the Wasteland</h3>
                                    <p
                                        class="font-body-md text-body-md text-on-surface-variant opacity-70 line-clamp-2">
                                        "Visualmente sorprendente, pero el ritmo se arrastra terriblemente en la segunda
                                        mitad. Una oportunidad perdida para una exploración temática más profunda."</p>
                                </div>
                                <div
                                    class="mt-6 flex items-center justify-between border-t border-outline-variant/10 pt-4">
                                    <div class="flex items-center gap-3">
                                        <div
                                            class="w-8 h-8 rounded-full bg-primary-container text-on-primary-container flex items-center justify-center font-label-md text-label-md shadow-sm">
                                            MV</div>
                                        <div>
                                            <div class="font-label-md text-label-md text-on-surface">Marcus Vance</div>
                                            <div class="font-caption text-caption text-on-surface-variant opacity-60">
                                                hace 5 horas</div>
                                        </div>
                                    </div>
                                </div>
                            </article>
                            <article
                                class="bg-surface-container rounded-2xl p-6 shadow-md hover:shadow-xl transition-all duration-300 relative overflow-hidden group cursor-pointer flex flex-col justify-between min-h-[240px]">
                                <div
                                    class="absolute -right-12 -top-12 w-32 h-32 bg-secondary-container/5 rounded-full blur-2xl group-hover:bg-secondary-container/10 transition-colors">
                                </div>
                                <div>
                                    <div class="flex items-center justify-between mb-6">
                                        <span
                                            class="flex items-center gap-2 text-secondary-container bg-secondary-container/10 px-3 py-1 rounded-full font-label-md text-label-md shadow-sm">
                                            <span class="material-symbols-outlined text-[16px]">share</span>
                                            Compartido
                                        </span>
                                        <div class="flex items-center gap-1 text-secondary-container">
                                            <span class="material-symbols-outlined text-[16px]"
                                                style="font-variation-settings: 'FILL' 1;">star</span>
                                            <span class="font-label-md text-label-md">4.2</span>
                                        </div>
                                    </div>
                                    <h3
                                        class="font-headline-md text-headline-md text-on-surface mb-2 group-hover:text-primary transition-colors line-clamp-1">
                                        Velvet Shadows</h3>
                                    <p
                                        class="font-body-md text-body-md text-on-surface-variant opacity-70 line-clamp-2">
                                        "Una brillante versión moderna de la clásica película de giallo. Violencia
                                        estilizada e implacable envuelta en una banda sonora impecable."</p>
                                </div>
                                <div
                                    class="mt-6 flex items-center justify-between border-t border-outline-variant/10 pt-4">
                                    <div class="flex items-center gap-3">
                                        <div
                                            class="w-8 h-8 rounded-full bg-primary-container text-on-primary-container flex items-center justify-center font-label-md text-label-md shadow-sm">
                                            SW
                                        </div>
                                        <div>
                                            <div class="font-label-md text-label-md text-on-surface">Sarah Wei</div>
                                            <div class="font-caption text-caption text-on-surface-variant opacity-60">
                                                hace 1 semana</div>
                                        </div>
                                    </div>
                                </div>
                            </article>
                        </div>
                    </div>
                    <aside class="w-full lg:w-1/4 flex flex-col gap-8">
                        <div class="bg-surface-container-high rounded-2xl p-6 shadow-lg">
                            <h3 class="font-headline-md text-headline-md text-on-surface mb-6 flex items-center gap-3">
                                <span class="material-symbols-outlined text-primary">analytics</span>
                                Estadísticas de Red
                            </h3>
                            <div class="space-y-6">
                                <div>
                                    <div class="flex justify-between text-label-md font-label-md mb-2">
                                        <span class="text-on-surface-variant">Top Compartidor</span>
                                        <span class="text-on-surface">Elena Rostova</span>
                                    </div>
                                    <div class="w-full bg-surface-variant rounded-full h-2 shadow-inner">
                                        <div class="bg-primary h-2 rounded-full" style="width: 65%"></div>
                                    </div>
                                </div>
                                <div>
                                    <div class="flex justify-between text-label-md font-label-md mb-2">
                                        <span class="text-on-surface-variant">Calificación Promedio</span>
                                        <span class="text-secondary-container flex items-center"><span
                                                class="material-symbols-outlined text-[14px] mr-1"
                                                style="font-variation-settings: 'FILL' 1;">star</span> 4.1</span>
                                    </div>
                                    <div class="w-full bg-surface-variant rounded-full h-2 shadow-inner">
                                        <div class="bg-secondary-container h-2 rounded-full" style="width: 82%"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="bg-surface-container rounded-2xl p-6 shadow-lg relative overflow-hidden">
                            <div
                                class="absolute right-0 top-0 w-32 h-32 bg-primary/10 rounded-bl-full mix-blend-screen pointer-events-none">
                            </div>
                            <h3 class="font-headline-md text-headline-md text-on-surface mb-4">Listas Curadas</h3>
                            <p class="font-body-md text-body-md text-on-surface-variant opacity-80 mb-6">Descubre
                                colecciones compiladas por los mejores críticos en tu red.</p>
                            <button
                                class="w-full py-3 px-6 rounded-lg bg-surface-variant text-on-surface font-label-md text-label-md uppercase tracking-wider hover:bg-primary-container hover:text-on-primary-container transition-colors shadow-md flex items-center justify-center gap-2 group">
                                Buscar Colecciones
                                <span
                                    class="material-symbols-outlined group-hover:translate-x-1 transition-transform">arrow_forward</span>
                            </button>
                        </div>
                    </aside>
                </div>
            </div>
        </main>
    </div>
</template>