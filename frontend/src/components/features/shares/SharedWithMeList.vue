<template>
        <aside
            class="fixed left-0 top-0 h-full w-72 bg-surface-container-lowest z-50 flex flex-col border-r border-outline-variant/10 shadow-2xl">
            <div class="px-8 py-10 flex items-center gap-3">
            <div class="h-8 w-8 bg-primary rounded flex items-center justify-center text-on-primary"><span
                class="material-symbols-outlined">movie</span></div><span
                class="font-headline-md text-headline-md tracking-tight text-on-surface uppercase">CineCraft</span>
            </div>
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
                class="material-symbols-outlined mr-4 group-hover:text-on-surface">archive</span>Archivados</a><a
                class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
                data-path="shared-with-me" href="#"><span
                class="material-symbols-outlined mr-4 group-hover:text-on-surface">group</span>Compartido Conmigo</a><a
                aria-current="page"
                class="flex items-center px-4 py-3 rounded-lg transition-all group bg-primary-container text-on-primary-container shadow-lg shadow-primary-container/20"
                data-path="my-shared" href="#"><span
                class="material-symbols-outlined mr-4 group-hover:text-on-surface">share</span>Mis Compartidas</a>
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
                class="material-symbols-outlined mr-4 group-hover:text-on-surface">analytics</span>Informes de
                Administrador</a>
            </nav>
            <div class="px-4 py-6 border-t border-outline-variant/10"><a
                class="flex items-center px-4 py-3 rounded-lg text-on-surface-variant hover:bg-surface-container-high hover:text-on-surface transition-all group"
                data-path="profile" href="#"><span
                class="material-symbols-outlined mr-4 group-hover:text-on-surface">person</span>Ajustes de Perfil</a></div>
        </aside>
        <div class="pl-72 min-h-screen flex flex-col">
            <header
            class="fixed top-0 left-72 right-0 h-20 bg-surface/80 backdrop-blur-2xl z-40 px-8 flex items-center justify-between border-b border-outline-variant/5 shadow-sm">
            <div class="flex-1 max-w-2xl relative group"><span
                class="material-symbols-outlined absolute left-4 top-1/2 -translate-y-1/2 text-on-surface-variant group-focus-within:text-primary transition-colors">search</span><input
                class="w-full bg-surface-container-high/50 border border-outline-variant/10 rounded-full py-2.5 pl-12 pr-6 text-on-surface placeholder:text-on-surface-variant/50 focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all font-body-md"
                placeholder="Buscar películas, reseñas o creadores..." type="text" /></div>
            <div class="flex items-center gap-6"><button
                class="relative p-2 rounded-full hover:bg-surface-variant transition-colors text-on-surface-variant hover:text-on-surface"><span
                    class="material-symbols-outlined">notifications</span><span
                    class="absolute top-2 right-2 w-2 h-2 bg-primary rounded-full"></span></button>
                <div class="flex items-center gap-3 pl-4 border-l border-outline-variant/20 hover:cursor-pointer group">
                <div class="text-right hidden sm:block">
                    <div class="text-label-md font-label-md text-on-surface">Alex Rivera</div>
                    <div class="text-caption font-caption text-on-surface-variant opacity-70">Crítico Profesional</div>
                </div>
                <div
                    class="w-10 h-10 rounded-full bg-surface-variant flex items-center justify-center font-bold text-on-surface ring-2 ring-transparent group-hover:ring-primary transition-all">
                    AR</div>
                </div>
            </div>
            </header>
            <main class="flex-1 pt-20 bg-surface p-8">
            <div class="flex flex-col w-full h-full relative font-body-md">
                <div class="mb-10 w-full flex items-center justify-between">
                <div>
                    <h1 class="font-display-lg text-display-lg text-on-surface mb-2">Mis Compartidas</h1>
                    <p class="font-body-lg text-body-lg text-on-surface-variant max-w-3xl">Haz un seguimiento de las
                    experiencias cinematográficas que has recomendado. Mira quién ha interactuado con tus reseñas compartidas
                    y notas destacadas.</p>
                </div>
                <button
                    class="flex items-center gap-2 bg-primary text-on-primary px-6 py-3 rounded-full font-label-md text-label-md hover:bg-primary-fixed transition-colors shadow-lg shadow-primary/20 hover:-translate-y-1 transform duration-300"
                    id="open-share-modal">
                    <span class="material-symbols-outlined">share</span>
                    <span>Compartir Nuevo</span>
                </button>
                </div>
                <div class="bg-surface-container rounded-xl overflow-hidden shadow-2xl relative z-10 w-full">
                <div
                    class="grid grid-cols-12 gap-4 px-8 py-5 bg-surface-container-high/50 border-b border-outline-variant/10 font-label-md text-label-md text-on-surface-variant uppercase tracking-widest text-xs">
                    <div class="col-span-4">Título de Película y Reseña</div>
                    <div class="col-span-3">Destinatario(s)</div>
                    <div class="col-span-2">Fecha de Envío</div>
                    <div class="col-span-2">Estado</div>
                    <div class="col-span-1 text-right">Acción</div>
                </div>
                <div class="divide-y divide-outline-variant/5">
                    <div
                    class="grid grid-cols-12 gap-4 px-8 py-6 items-center hover:bg-surface-container-high/30 transition-colors group">
                    <div class="col-span-4 flex gap-4 items-center">
                        <div class="w-16 h-24 rounded-lg overflow-hidden shrink-0 shadow-md">
                        <div
                            class="w-full h-full bg-surface-variant flex items-center justify-center text-on-surface-variant group-hover:scale-105 transition-transform duration-500">
                            <span class="material-symbols-outlined text-3xl">movie</span></div>
                        </div>
                        <div>
                        <h3
                            class="font-headline-md text-body-lg text-on-surface font-semibold mb-1 group-hover:text-primary transition-colors">
                            Blade Runner 2049</h3>
                        <p class="text-caption font-caption text-on-surface-variant line-clamp-2">"La obra maestra de
                            Villeneuve es visualmente deslumbrante. El uso de la escala y el color crea una atmósfera opresiva
                            pero hermosa que perdura..."</p>
                        </div>
                    </div>
                    <div class="col-span-3 flex -space-x-3">
                        <div
                        class="w-8 h-8 rounded-full border-2 border-surface-container bg-surface-variant flex items-center justify-center text-xs font-bold text-on-surface z-30">
                        SJ</div>
                        <div
                        class="w-8 h-8 rounded-full border-2 border-surface-container bg-surface-variant flex items-center justify-center text-xs font-bold text-on-surface z-20">
                        MT</div>
                        <div
                        class="w-8 h-8 rounded-full border-2 border-surface-container bg-surface-variant flex items-center justify-center text-caption font-caption text-on-surface z-10">
                        +2</div>
                    </div>
                    <div class="col-span-2 flex items-center gap-2">
                        <span class="material-symbols-outlined text-[16px] text-on-surface-variant">calendar_today</span>
                        <span class="font-body-md text-sm text-on-surface-variant">Oct 12, 2023</span>
                    </div>
                    <div class="col-span-2 flex items-center">
                        <div
                        class="flex items-center gap-2 px-3 py-1 bg-secondary-container/20 text-secondary-container rounded-full w-fit">
                        <span class="material-symbols-outlined text-[14px]">visibility</span>
                        <span class="font-label-md text-xs uppercase tracking-wider">Visto por 3</span>
                        </div>
                    </div>
                    <div class="col-span-1 flex justify-end">
                        <button
                        class="p-2 text-on-surface-variant hover:text-primary transition-colors rounded-full hover:bg-surface-variant">
                        <span class="material-symbols-outlined">more_vert</span>
                        </button>
                    </div>
                    </div>
                    <div
                    class="grid grid-cols-12 gap-4 px-8 py-6 items-center hover:bg-surface-container-high/30 transition-colors group">
                    <div class="col-span-4 flex gap-4 items-center">
                        <div class="w-16 h-24 rounded-lg overflow-hidden shrink-0 shadow-md">
                        <div
                            class="w-full h-full bg-surface-variant flex items-center justify-center text-on-surface-variant group-hover:scale-105 transition-transform duration-500">
                            <span class="material-symbols-outlined text-3xl">movie</span></div>
                        </div>
                        <div>
                        <h3
                            class="font-headline-md text-body-lg text-on-surface font-semibold mb-1 group-hover:text-primary transition-colors">
                            Past Lives</h3>
                        <p class="text-caption font-caption text-on-surface-variant line-clamp-2">"Una delicada exploración de
                            lo que podría haber sido. El acto final es una clase magistral de emoción contenida y narración
                            sutil."</p>
                        </div>
                    </div>
                    <div class="col-span-3 flex -space-x-3">
                        <div
                        class="w-8 h-8 rounded-full border-2 border-surface-container bg-surface-variant flex items-center justify-center text-xs font-bold text-on-surface z-30">
                        DC</div>
                    </div>
                    <div class="col-span-2 flex items-center gap-2">
                        <span class="material-symbols-outlined text-[16px] text-on-surface-variant">calendar_today</span>
                        <span class="font-body-md text-sm text-on-surface-variant">Nov 05, 2023</span>
                    </div>
                    <div class="col-span-2 flex items-center">
                        <div
                        class="flex items-center gap-2 px-3 py-1 bg-surface-variant text-on-surface-variant rounded-full w-fit">
                        <span class="material-symbols-outlined text-[14px]">visibility_off</span>
                        <span class="font-label-md text-xs uppercase tracking-wider">No Visto</span>
                        </div>
                    </div>
                    <div class="col-span-1 flex justify-end">
                        <button
                        class="p-2 text-on-surface-variant hover:text-primary transition-colors rounded-full hover:bg-surface-variant">
                        <span class="material-symbols-outlined">more_vert</span>
                        </button>
                    </div>
                    </div>
                    <div
                    class="grid grid-cols-12 gap-4 px-8 py-6 items-center hover:bg-surface-container-high/30 transition-colors group">
                    <div class="col-span-4 flex gap-4 items-center">
                        <div class="w-16 h-24 rounded-lg overflow-hidden shrink-0 shadow-md">
                        <div
                            class="w-full h-full bg-surface-variant flex items-center justify-center text-on-surface-variant group-hover:scale-105 transition-transform duration-500">
                            <span class="material-symbols-outlined text-3xl">movie</span></div>
                        </div>
                        <div>
                        <h3
                            class="font-headline-md text-body-lg text-on-surface font-semibold mb-1 group-hover:text-primary transition-colors">
                            The Lighthouse</h3>
                        <p class="text-caption font-caption text-on-surface-variant line-clamp-2">"Claustrofóbica y
                            desquiciada. El diseño de sonido por sí solo merece un ensayo. Dafoe y Pattinson están fenomenales."
                        </p>
                        </div>
                    </div>
                    <div class="col-span-3 flex -space-x-3">
                        <div
                        class="w-8 h-8 rounded-full border-2 border-surface-container bg-surface-variant flex items-center justify-center text-xs font-bold text-on-surface z-30">
                        SJ</div>
                        <div
                        class="w-8 h-8 rounded-full border-2 border-surface-container bg-surface-variant flex items-center justify-center text-xs font-bold text-on-surface z-20">
                        ER</div>
                    </div>
                    <div class="col-span-2 flex items-center gap-2">
                        <span class="material-symbols-outlined text-[16px] text-on-surface-variant">calendar_today</span>
                        <span class="font-body-md text-sm text-on-surface-variant">Nov 18, 2023</span>
                    </div>
                    <div class="col-span-2 flex items-center">
                        <div
                        class="flex items-center gap-2 px-3 py-1 bg-secondary-container/20 text-secondary-container rounded-full w-fit">
                        <span class="material-symbols-outlined text-[14px]">visibility</span>
                        <span class="font-label-md text-xs uppercase tracking-wider">Visto por 1</span>
                        </div>
                    </div>
                    <div class="col-span-1 flex justify-end">
                        <button
                        class="p-2 text-on-surface-variant hover:text-primary transition-colors rounded-full hover:bg-surface-variant">
                        <span class="material-symbols-outlined">more_vert</span>
                        </button>
                    </div>
                    </div>
                </div>
                <div
                    class="px-8 py-4 border-t border-outline-variant/10 flex justify-between items-center bg-surface-container-high/20 text-sm font-body-md text-on-surface-variant">
                    <span>Mostrando 1 a 3 de 12 entradas</span>
                    <div class="flex gap-2">
                    <button class="p-1 rounded hover:bg-surface-variant transition-colors disabled:opacity-50"><span
                        class="material-symbols-outlined">chevron_left</span></button>
                    <button class="p-1 rounded hover:bg-surface-variant transition-colors"><span
                        class="material-symbols-outlined">chevron_right</span></button>
                    </div>
                </div>
                </div>
                <div
                class="fixed inset-0 z-50 flex items-center justify-center opacity-0 pointer-events-none transition-opacity duration-300"
                id="share-modal">
                <div class="absolute inset-0 bg-background/80 backdrop-blur-xl transition-opacity" id="modal-backdrop"></div>
                <div
                    class="bg-surface-container-high w-full max-w-lg rounded-2xl shadow-2xl relative z-10 flex flex-col max-h-[870px] transform scale-95 transition-transform duration-300"
                    id="modal-content">
                    <div class="px-6 py-5 border-b border-outline-variant/10 flex justify-between items-center">
                    <div>
                        <h2 class="font-headline-md text-headline-md text-on-surface">Compartir Reseña</h2>
                        <p class="text-caption font-caption text-on-surface-variant mt-1">Selecciona los usuarios a los que
                        enviar tu destacado.</p>
                    </div>
                    <button class="p-2 rounded-full hover:bg-surface-variant text-on-surface-variant transition-colors"
                        id="close-share-modal">
                        <span class="material-symbols-outlined">close</span>
                    </button>
                    </div>
                    <div class="p-4 border-b border-outline-variant/10 bg-surface-container/50">
                    <div class="relative group">
                        <span
                        class="material-symbols-outlined absolute left-4 top-1/2 -translate-y-1/2 text-on-surface-variant group-focus-within:text-primary transition-colors">search</span>
                        <input
                        class="w-full bg-surface-variant/50 border border-outline-variant/20 rounded-xl py-3 pl-12 pr-6 text-on-surface placeholder:text-on-surface-variant/50 focus:outline-none focus:ring-2 focus:ring-primary/30 focus:border-primary transition-all font-body-md"
                        placeholder="Buscar por nombre o correo..." type="text" />
                    </div>
                    </div>
                    <div class="flex-1 overflow-y-auto p-2 space-y-1">
                    <label
                        class="flex items-center gap-4 p-3 rounded-xl hover:bg-surface-variant/50 cursor-pointer transition-colors group">
                        <div class="relative flex items-center">
                        <input class="peer sr-only" type="checkbox" />
                        <div
                            class="w-5 h-5 border-2 border-outline-variant rounded peer-checked:bg-primary peer-checked:border-primary transition-colors flex items-center justify-center">
                            <span
                            class="material-symbols-outlined text-[16px] text-on-primary opacity-0 peer-checked:opacity-100 scale-50 peer-checked:scale-100 transition-all duration-200">check</span>
                        </div>
                        </div>
                        <div
                        class="w-10 h-10 rounded-full bg-surface-variant flex items-center justify-center font-bold text-sm text-on-surface">
                        SJ</div>
                        <div class="flex-1 min-w-0">
                        <div
                            class="font-label-md text-sm text-on-surface truncate group-hover:text-primary transition-colors">
                            Sarah Jenkins</div>
                        <div class="font-caption text-caption text-on-surface-variant truncate">@sjenkins_film</div>
                        </div>
                    </label>
                    <label
                        class="flex items-center gap-4 p-3 rounded-xl hover:bg-surface-variant/50 cursor-pointer transition-colors group">
                        <div class="relative flex items-center">
                        <input class="peer sr-only" type="checkbox" />
                        <div
                            class="w-5 h-5 border-2 border-outline-variant rounded peer-checked:bg-primary peer-checked:border-primary transition-colors flex items-center justify-center">
                            <span
                            class="material-symbols-outlined text-[16px] text-on-primary opacity-0 peer-checked:opacity-100 scale-50 peer-checked:scale-100 transition-all duration-200">check</span>
                        </div>
                        </div>
                        <div
                        class="w-10 h-10 rounded-full bg-surface-variant flex items-center justify-center font-bold text-sm text-on-surface">
                        MT</div>
                        <div class="flex-1 min-w-0">
                        <div
                            class="font-label-md text-sm text-on-surface truncate group-hover:text-primary transition-colors">
                            Marcus Thorne</div>
                        <div class="font-caption text-caption text-on-surface-variant truncate">@marcust_dop</div>
                        </div>
                    </label>
                    <label
                        class="flex items-center gap-4 p-3 rounded-xl hover:bg-surface-variant/50 cursor-pointer transition-colors group">
                        <div class="relative flex items-center">
                        <input checked="" class="peer sr-only" type="checkbox" />
                        <div
                            class="w-5 h-5 border-2 border-outline-variant rounded peer-checked:bg-primary peer-checked:border-primary transition-colors flex items-center justify-center">
                            <span
                            class="material-symbols-outlined text-[16px] text-on-primary opacity-0 peer-checked:opacity-100 scale-50 peer-checked:scale-100 transition-all duration-200">check</span>
                        </div>
                        </div>
                        <div
                        class="w-10 h-10 rounded-full bg-secondary-container text-on-secondary-container flex items-center justify-center font-headline-md text-sm">
                        EL</div>
                        <div class="flex-1 min-w-0">
                        <div
                            class="font-label-md text-sm text-on-surface truncate group-hover:text-primary transition-colors">
                            Elena Rodriguez</div>
                        <div class="font-caption text-caption text-on-surface-variant truncate">elena.r@cinecraft.app</div>
                        </div>
                    </label>
                    <label
                        class="flex items-center gap-4 p-3 rounded-xl hover:bg-surface-variant/50 cursor-pointer transition-colors group">
                        <div class="relative flex items-center">
                        <input class="peer sr-only" type="checkbox" />
                        <div
                            class="w-5 h-5 border-2 border-outline-variant rounded peer-checked:bg-primary peer-checked:border-primary transition-colors flex items-center justify-center">
                            <span
                            class="material-symbols-outlined text-[16px] text-on-primary opacity-0 peer-checked:opacity-100 scale-50 peer-checked:scale-100 transition-all duration-200">check</span>
                        </div>
                        </div>
                        <div
                        class="w-10 h-10 rounded-full bg-surface-variant flex items-center justify-center font-bold text-sm text-on-surface">
                        DC</div>
                        <div class="flex-1 min-w-0">
                        <div
                            class="font-label-md text-sm text-on-surface truncate group-hover:text-primary transition-colors">
                            David Chen</div>
                        <div class="font-caption text-caption text-on-surface-variant truncate">@dchen_edits</div>
                        </div>
                    </label>
                    </div>
                    <div
                    class="p-6 border-t border-outline-variant/10 bg-surface-container-high flex justify-between items-center rounded-b-2xl">
                    <div class="text-sm font-body-md text-on-surface-variant">
                        <span class="font-bold text-on-surface">1</span> usuario seleccionado
                    </div>
                    <button
                        class="bg-primary text-on-primary px-8 py-2.5 rounded-full font-label-md text-label-md hover:bg-primary-fixed transition-all shadow-md shadow-primary/20 hover:shadow-primary/40 focus:ring-2 focus:ring-primary focus:ring-offset-2 focus:ring-offset-surface-container-high">
                        Compartir
                    </button>
                    </div>
                </div>
                </div>
                <script>
                document.addEventListener('DOMContentLoaded', () => {
                    const modal = document.getElementById('share-modal');
                    const modalContent = document.getElementById('modal-content');
                    const openBtn = document.getElementById('open-share-modal');
                    const closeBtn = document.getElementById('close-share-modal');
                    const backdrop = document.getElementById('modal-backdrop');

                    function openModal() {
                    modal.classList.remove('opacity-0', 'pointer-events-none');
                    setTimeout(() => {
                        modalContent.classList.remove('scale-95');
                        modalContent.classList.add('scale-100');
                    }, 10);
                    }

                    function closeModal() {
                    modalContent.classList.remove('scale-100');
                    modalContent.classList.add('scale-95');
                    setTimeout(() => {
                        modal.classList.add('opacity-0', 'pointer-events-none');
                    }, 300);
                    }

                    openBtn.addEventListener('click', openModal);
                    closeBtn.addEventListener('click', closeModal);
                    backdrop.addEventListener('click', closeModal);
                });
                </script>
            </div>
            </main>
        </div>
</template>