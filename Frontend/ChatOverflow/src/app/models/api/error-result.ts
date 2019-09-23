export interface IErrorResult {
    data: any;
    errors: Array<IErrorCodeResult>;
}

export interface IErrorCodeResult {
    code: string;
    description: string;
}