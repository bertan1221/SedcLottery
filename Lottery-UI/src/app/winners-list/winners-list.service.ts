import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { IUserCodeAward } from "./winners-list.model";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";

@Injectable({
    providedIn: 'root'
})
export class WinnersListService {
    constructor(private http: HttpClient) {}

    getAllWinners(): Observable<Array<IUserCodeAward>> {
        return this.http.get<Array<IUserCodeAward>>(environment.webApiUrl + "getAllWinners");
    }
}