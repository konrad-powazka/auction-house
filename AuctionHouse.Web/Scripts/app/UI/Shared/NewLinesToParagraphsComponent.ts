import { INamedComponentOptions } from '../../Infrastructure/INamedComponentOptions';
import { NewLinesToParagraphsCtrl } from './NewLinesToParagraphsCtrl';

export class NewLinesToParagraphsComponent implements INamedComponentOptions {
	controller = NewLinesToParagraphsCtrl;
	registerAs = 'newLinesToParagraphs';
    bindings = {
		text: '<'
    }
}