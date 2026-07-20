import { ConfigStateService, RoutesService, eLayoutType } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routes = inject(RoutesService);
  const configState = inject(ConfigStateService);

  let menuAdminAgregado = false;
  configState.getOne$('currentUser').subscribe(user => {
    if (!menuAdminAgregado && user?.roles?.includes('admin')) {
      menuAdminAgregado = true;
      routes.add([
        {
          path: '/admin/metricas',
          name: 'Métricas API',
          iconClass: 'fas fa-chart-bar',
          order: 5,
          layout: eLayoutType.application,
        },
      ]);
    }
  });
  routes.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/ciudades',
        name: 'Ciudades',
        iconClass: 'fas fa-city',
        order: 2,
        layout: eLayoutType.application,
      },
      {
        path: '/populares',
        name: 'Populares',
        iconClass: 'fas fa-star',
        order: 3,
        layout: eLayoutType.application,
      },
      {
        path: '/mi-perfil',
        name: 'Mi perfil',
        iconClass: 'fas fa-user',
        order: 4,
        layout: eLayoutType.application,
      },
  ]);
}
