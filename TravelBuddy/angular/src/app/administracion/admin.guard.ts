import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ConfigStateService } from '@abp/ng.core';

export const adminGuard: CanActivateFn = () => {
  const configState = inject(ConfigStateService);
  const router = inject(Router);

  const currentUser = configState.getOne('currentUser');
  if (currentUser?.isAuthenticated && currentUser.roles?.includes('admin')) {
    return true;
  }

  return router.createUrlTree(['/']);
};
