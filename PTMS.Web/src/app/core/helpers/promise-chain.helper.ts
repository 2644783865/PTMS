export interface PromiseChainResult {
    successResponses: any[]
    errors: any[]
}

export class PromiseChainHelper {
    private _promiseFunctions: (() => Promise<any>)[] = [];

    constructor() {}

    add(func: () => Promise<any>) {
        this._promiseFunctions.push(func);
    }

    /*
    Run promises one by one
    */
    run(): Promise<PromiseChainResult> {
        let array = this._promiseFunctions;

        let result = { 
            successResponses: [], 
            errors: []
        } as PromiseChainResult;

        if (array.length == 0) {
            return Promise.resolve(result);
        }

        let resultPromise = new Promise<PromiseChainResult>((resolve) => {
            let count = 0;

            let runPromise = () => {
                let promise = array[count]();
    
                promise.then(resp => {
                    result.successResponses.push(resp);
                })
                .catch(err => {
                    result.errors.push(err);
                })
                .finally(() => {
                    count++;
    
                    if (count != array.length) {
                        runPromise();
                    }
                    else {
                        resolve(result);
                    }
                })
            }

            runPromise();
        })

        return resultPromise;
    }
}