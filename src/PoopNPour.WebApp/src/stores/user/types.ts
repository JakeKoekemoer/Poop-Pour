import type { PoopNPourApi } from '@/api/api-client';

/**
 * User authentication state
 */
export interface UserState {
  /** JWT authentication token */
  token: string | null;
  /** Authenticated user information */
  user: UserProfile | null;
  /** Token expiration timestamp */
  expiresAt: Date | null;
}

/**
 * User profile information
 */
export interface UserProfile {
  id: string;
  email: string;
  userName: string;
  firstName?: string;
  lastName?: string;
  roles: string[];
  createdAt: Date;
  updatedAt?: Date;
}

/**
 * Converts API UserDto to UserProfile
 */
export function toUserProfile(dto: PoopNPourApi.UserDto): UserProfile {
  return {
    id: dto.id || '',
    email: dto.email || '',
    userName: dto.userName || '',
    firstName: dto.firstName,
    lastName: dto.lastName,
    roles: dto.roles || [],
    createdAt: dto.createdAt || new Date(),
    updatedAt: dto.updatedAt,
  };
}
