import { authGuard, permissionGuard } from '@abp/ng.core';
import { Routes } from '@angular/router';
import { CiudadesComponent } from './ciudades/ciudades';
import { CiudadDetalle } from './ciudades/ciudad-detalle/ciudad-detalle';
import { CiudadesPopulares } from './ciudades/ciudades-populares/ciudades-populares';
import { MiPerfil } from './perfil/mi-perfil/mi-perfil';
import { PerfilPublico } from './perfil/perfil-publico/perfil-publico';
import { PanelMetricas } from './administracion/panel-metricas/panel-metricas';
import { adminGuard } from './administracion/admin.guard';
import { Component } from '@angular/core';

export const APP_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./home/home.component').then(c => c.HomeComponent),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(c => c.createRoutes()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(c => c.createRoutes()),
  },
  {
    path: 'setting-management',
    loadChildren: () => import('@abp/ng.setting-management').then(c => c.createRoutes()),
  },
  { 
    path: 'ciudades', 
    component: CiudadesComponent,
    canActivate: [authGuard]
  },
  {
    path: 'ciudades/detalle/:id',
    component: CiudadDetalle
  },
  {
    path: 'populares',
    component: CiudadesPopulares
  },
  {
    path: 'mi-perfil',
    component: MiPerfil,
    canActivate: [authGuard]
  },
  {
    path: 'perfil/:userId',
    component: PerfilPublico
  },
  {
    path: 'admin/metricas',
    component: PanelMetricas,
    canActivate: [authGuard, adminGuard]
  }

];
