import { ActionResult } from "@/types/ActionResult";
import axios, { AxiosResponse } from "axios";

export const SERVER = axios.create({
  baseURL: "http://cclapps.eastus.cloudapp.azure.com:55555/",
  // baseURL: 'https://localhost:55555/'
});

export class ErrorHandler<T> {
  // eslint-disable-next-line
  private error!: any;
  constructor(response: AxiosResponse) {
    this.error = response;
  }

  public extractErrorMessages(): string[] {
    if (this.error.response) {
      console.log(this.error);

      const status = this.error?.response?.status;

      if (status) {
        if (status == 500) {
          return [
            `The server encountered an unexpected condition that prevented it from fulfilling this request`,
          ];
        }
        if (status == 400) {
          return [
            `An invalid request parameter was sent for processing by the request.`,
          ];
        }
      }
      const response = this.error.response.data as ActionResult<T>;
      return response.errors;
    }

    const serverResponse = this.error?.data as ActionResult<T>;
    const errors = serverResponse?.errors;

    if (errors) return errors;

    return [this.error];
  }
}

export default SERVER;
