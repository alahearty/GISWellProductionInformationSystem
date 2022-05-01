import { IndexState } from "@/modules/Index/store";
import { AuthState } from '@/views/Auth/store';

export interface ApplicationState {
    index: IndexState;
    auth: AuthState;
}