export class NewLinesToParagraphsCtrl implements ng.IController {
	text: string;

	static $inject = ['$element', '$scope'];

	constructor(element: JQuery, scope: ng.IScope) {
		scope.$watch(() => this.text,
		() => {
			element.empty();

			if (!_(this.text).isString()) {
				return;
			}

			var lines = this.text.split(/\r\n|\r|\n/);
			const paragraphElements: any = _(lines).map(line => $('<p></p>').text(line));

			for (const paragraphElement of paragraphElements) {
				element.append(paragraphElement);
			}
		});
	}
}