export interface ActionResult<T> {
    data: T
    wasSuccesful: boolean;
    errors: string[];
}

