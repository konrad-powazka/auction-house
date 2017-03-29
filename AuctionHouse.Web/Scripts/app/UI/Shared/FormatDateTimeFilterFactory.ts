export default class FormatDateTimeFilterFactory {
    static $inject = [''];

    static createFilterFunction() {
	    return (value: any) => {
		    return moment(value).format('Do MMMM YYYY, h:mm A');
	    }
    }
}