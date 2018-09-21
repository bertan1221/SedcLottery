import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { IUserCode, IAward } from "../winners-list/winners-list.model";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";

@Injectable({
    providedIn: 'root'
})
export class SubmitCodeService {
    constructor(private http: HttpClient) {}

    submitCode(userCode: IUserCode) : Observable<IAward> {
        return this.http.post<IAward>(environment.webApiUrl + 'submitCode', userCode);
    }
}