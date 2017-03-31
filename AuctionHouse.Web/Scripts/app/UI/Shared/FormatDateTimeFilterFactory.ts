export default class FormatDateTimeFilterFactory {
	static $inject = [''];

	static createStandardFilterFunction(): (value: any) => (string | null) {
		return (value: any): (string | null) => {
			if (!value) {
				return null;
			}

			return moment(value).format('Do MMM YYYY, h:mm A');
		}
	}

	static createToNowFilterFunction(): (value: any) => (string | null) {
		return (value: any): (string | null) => {
			if (!value) {
				return null;
			}

			return moment(value).toNow(true);
		}
	}
}