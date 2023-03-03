import { AuthService } from '@app/_services';

export function appInitializer(authService: AuthService) {
    return () => new Promise(
        resolve => {
            authService.refreshToken()
                .subscribe()
                .add(resolve);
        }
    );
}