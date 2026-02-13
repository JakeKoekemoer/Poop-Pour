import { BaseService } from '../BaseService';
import type { ApiResponse } from '../types';
import type { PoopNPourApi } from '@/api/api-client';
import { useUserStore, type UserProfile } from '@/stores';
import { toUserProfile } from '@/stores/user/types';
import router from '@/routes';

/**
 * Authentication service for login, registration, and logout operations
 */
class AuthService extends BaseService {
  /**
   * Authenticate user with username and password
   * 
   * @param username - User's username
   * @param password - User's password
   * @returns Promise with login response containing token, expiresAt, and user
   */
  async login(
    username: string,
    password: string
  ): Promise<ApiResponse<{ token: string; expiresAt: Date; user: UserProfile }>> {
    const dto: PoopNPourApi.LoginRequestDto = {
      username,
      password,
    };

    const response = await this.execute(() => this.client.login(dto));

    if (response.success && response.data) {
      const { token, expiresAt, user } = response.data;
      
      if (token && user && expiresAt) {
        const userProfile = toUserProfile(user);
        const expiryDate = new Date(expiresAt);
        
        const userStore = useUserStore();
        userStore.setAuth(token, userProfile, expiryDate);

        return {
          success: true,
          data: {
            token,
            expiresAt: expiryDate,
            user: userProfile,
          },
        };
      }
    }

    return response as ApiResponse<{ token: string; expiresAt: Date; user: UserProfile }>;
  }

  /**
   * Register a new user account
   * 
   * @param email - User's email address
   * @param userName - User's username
   * @param password - User's password
   * @param firstName - Optional first name
   * @param lastName - Optional last name
   * @returns Promise with registration response containing token, expiresAt, and user
   */
  async register(
    email: string,
    userName: string,
    password: string,
    firstName?: string,
    lastName?: string
  ): Promise<ApiResponse<{ token: string; expiresAt: Date; user: UserProfile }>> {
    const dto: PoopNPourApi.RegisterRequestDto = {
      email,
      userName,
      password,
      firstName,
      lastName,
    };

    const response = await this.execute(() => this.client.register(dto));

    if (response.success && response.data) {
      const { token, expiresAt, user } = response.data;
      
      if (token && user && expiresAt) {
        const userProfile = toUserProfile(user);
        const expiryDate = new Date(expiresAt);
        
        const userStore = useUserStore();
        userStore.setAuth(token, userProfile, expiryDate);

        return {
          success: true,
          data: {
            token,
            expiresAt: expiryDate,
            user: userProfile,
          },
        };
      }
    }

    return response as ApiResponse<{ token: string; expiresAt: Date; user: UserProfile }>;
  }

  /**
   * Log out the current user
   * Clears authentication state and redirects to login page
   */
  logout(): void {
    const userStore = useUserStore();
    userStore.clearAuth();

    router.push({ name: 'public.login' }).catch(err => {
      console.error('Failed to redirect to login:', err);
    });
  }
}

export const authService = new AuthService();
