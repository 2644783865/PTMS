export class IntervalHelper {
    private isComponentDestroyed = false;
    private intervalId;

    constructor(private func: Function, private updateInterval: number){

    }

    startInterval() {
        if (!this.isComponentDestroyed) {
            let that = this;

            this.clearInterval();
        
            this.intervalId = setInterval(_ => {
              that.func();
            }, this.updateInterval);
        }
    }

    clearInterval() {
        if (this.intervalId) {
            clearInterval(this.intervalId);
        }
    }

    onComponentDestroy() {
        this.isComponentDestroyed = true;
        this.clearInterval();
    }
}