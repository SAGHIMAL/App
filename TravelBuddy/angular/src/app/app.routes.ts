import { authGuard, permissionGuard } from '@abp/ng.core';
import { Routes } from '@angular/router';
import { CiudadesComponent } from './ciudades/ciudades';
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
  { path: 'ciudades', 
    component: CiudadesComponent,
    canActivate: [authGuard]},
];
