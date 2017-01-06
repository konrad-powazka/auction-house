export class SecurityService {
    private currentUserName: string | null = null;

    static $inject = ['$http'];

    constructor(private httpService: ng.IHttpService) {
    }

    logIn(userName: string, password: string): ng.IPromise<void> {
        const loginCommand = {
            userName: userName,
            password: password
        };

        return this.httpService.post('api/Authentication/LogIn', loginCommand)
            .then(() => {
                this.currentUserName = userName;
            });
    }

    logOut(userName: string, password: string): ng.IPromise<void> {
        if (!this.checkIfUserIsAuthenticated()) {
            throw new Error('Current user is not authenticated.');
        }
        return this.httpService.post('api/Authentication/LogOut', {})
            .then(() => {
                this.currentUserName = null;
            });
    }

    checkIfUserIsAuthenticated(): boolean {
        return this.currentUserName !== null;
    }

    getCurrentUserName(): string {
        if (!this.checkIfUserIsAuthenticated()) {
            throw new Error('Current user is not authenticated.');
        }

        return this.currentUserName as string;
    }
}