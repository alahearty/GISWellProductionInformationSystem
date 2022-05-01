export default class Directory<T = {}> {
    name: string;
    type: 'file' | 'folder';
    fileExtension?: string;
    show: boolean;
    data?: T;
    children?: Directory[];

    constructor() {
        this.name = '';
        this.type = 'file';
        this.show = false;
        this.children = [];
    }
}

export class DirectoryOptions extends Directory {
    editName: boolean;
    showOptions: boolean;

    constructor() {
        super();
        this.editName = false;
        this.showOptions = false;
    }
}