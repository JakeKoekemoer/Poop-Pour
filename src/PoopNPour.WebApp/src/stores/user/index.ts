import { defineStore } from 'pinia';
import Cookies from 'js-cookie';
import type { UserState, UserProfile } from './types';
import { setAuthToken as setClientAuthToken, clearAuthToken as clearClientAuthToken } from '@/services/api';

export type { UserProfile } from './types';

const AUTH_TOKEN_COOKIE = 'poop-pour-auth-token';
const TOKEN_EXPIRY_COOKIE = 'poop-pour-token-expiry';
const USER_PROFILE_KEY = 'poop-pour-user-profile';
const COOKIE_OPTIONS = {
  sameSite: 'strict' as const,
  secure: import.meta.env.PROD,
};

export const useUserStore = defineStore('user', {
  state: (): UserState => ({
    token: null,
    user: null,
    expiresAt: null,
  }),

  getters: {
    isAuthenticated: (state): boolean => {
      if (!state.token || !state.user) {
        return false;
      }
      
      if (state.expiresAt && new Date() >= state.expiresAt) {
        return false;
      }
      
      return true;
    },

    fullName: (state): string => {
      if (!state.user) return '';
      const { firstName, lastName } = state.user;
      if (firstName && lastName) {
        return `${firstName} ${lastName}`;
      }
      return firstName || lastName || state.user.userName;
    },

    initials: (state): string => {
      if (!state.user) return '';
      const { firstName, lastName, userName } = state.user;
      
      if (firstName && lastName) {
        return `${firstName[0]}${lastName[0]}`.toUpperCase();
      }
      if (firstName) {
        return firstName.substring(0, 2).toUpperCase();
      }
      if (userName) {
        return userName.substring(0, 2).toUpperCase();
      }
      return '';
    },
  },

  actions: {
    setAuth(token: string, user: UserProfile, expiresAt: Date): void {
      this.token = token;
      this.user = user;
      this.expiresAt = expiresAt;

      const expiryDays = Math.ceil(
        (expiresAt.getTime() - Date.now()) / (1000 * 60 * 60 * 24)
      );
      
      Cookies.set(AUTH_TOKEN_COOKIE, token, {
        ...COOKIE_OPTIONS,
        expires: expiryDays > 0 ? expiryDays : 1,
      });
      
      Cookies.set(TOKEN_EXPIRY_COOKIE, expiresAt.toISOString(), {
        ...COOKIE_OPTIONS,
        expires: expiryDays > 0 ? expiryDays : 1,
      });

      localStorage.setItem(USER_PROFILE_KEY, JSON.stringify(user));

      setClientAuthToken(token);
    },

    clearAuth(): void {
      this.token = null;
      this.user = null;
      this.expiresAt = null;

      Cookies.remove(AUTH_TOKEN_COOKIE);
      Cookies.remove(TOKEN_EXPIRY_COOKIE);
      localStorage.removeItem(USER_PROFILE_KEY);

      clearClientAuthToken();
    },

    initializeAuth(): void {
      if (typeof window === 'undefined') return;

      const token = Cookies.get(AUTH_TOKEN_COOKIE);
      const expiryStr = Cookies.get(TOKEN_EXPIRY_COOKIE);

      if (token && expiryStr) {
        const expiresAt = new Date(expiryStr);
        
        if (new Date() >= expiresAt) {
          this.clearAuth();
          return;
        }

        const userJson = localStorage.getItem(USER_PROFILE_KEY);
        const user: UserProfile | null = userJson ? JSON.parse(userJson) : null;

        if (!user) {
          this.clearAuth();
          return;
        }

        this.token = token;
        this.expiresAt = expiresAt;
        this.user = user;
        
        setClientAuthToken(token);
      }
    },

    updateUser(user: UserProfile): void {
      this.user = user;
    },

    hasRole(role: string): boolean {
      return this.user?.roles?.includes(role) || false;
    },

    hasAnyRole(roles: string[]): boolean {
      if (!this.user?.roles) return false;
      return roles.some(role => this.user!.roles.includes(role));
    },

    hasAllRoles(roles: string[]): boolean {
      if (!this.user?.roles) return false;
      return roles.every(role => this.user!.roles.includes(role));
    },
  },
});
