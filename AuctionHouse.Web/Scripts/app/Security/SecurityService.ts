export class SecurityService {
	private currentUserName: string | null = null;

	static $inject = ['$http'];

	constructor(private httpService: ng.IHttpService) {
		this.httpService.get<any>('api/Authentication/GetCurrentUser', {})
			.then((response) => {
				if (!this.currentUserName && response.data && response.data.name) {
					this.currentUserName = response.data.name;
				}
			});
	}

	signIn(userName: string, password: string): ng.IPromise<void> {
		const loginCommand = {
			userName: userName,
			password: password
		};

		return this.httpService.post('api/Authentication/SignIn', loginCommand)
			.then(() => {
				this.currentUserName = userName;
			});
	}

	signOut(): ng.IPromise<void> {
		if (!this.checkIfUserIsAuthenticated()) {
			throw new Error('Current user is not authenticated.');
		}

		return this.httpService.post('api/Authentication/SignOut', {})
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