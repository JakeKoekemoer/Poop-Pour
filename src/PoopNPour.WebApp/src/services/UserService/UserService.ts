import { BaseService } from "../BaseService";
import type { ApiResponse } from "../types";
import type {
  UpdateProfileRequestDto,
  CreateUserCommand,
  UpdateUserCommand,
  ClaimDto,
  AddUserRoleCommand,
} from "@/api/api-client";
import { useUserStore, type UserProfile } from "@/stores";
import { toUserProfile } from "@/stores/user/types";

class UserService extends BaseService {
  async getMyProfile(): Promise<ApiResponse<UserProfile>> {
    const response = await this.execute(() => this.client.getMyProfile());

    if (response.success && response.data) {
      const userProfile = toUserProfile(response.data);

      const userStore = useUserStore();
      userStore.updateUser(userProfile);

      return {
        success: true,
        data: userProfile,
      };
    }

    return response as ApiResponse<UserProfile>;
  }

  async updateMyProfile(
    firstName?: string,
    lastName?: string,
    email?: string,
  ): Promise<ApiResponse<UserProfile>> {
    const dto: UpdateProfileRequestDto = {
      firstName,
      lastName,
      email,
    };

    const response = await this.execute(() => this.client.updateMyProfile(dto));

    if (response.success && response.data) {
      const userProfile = toUserProfile(response.data);

      const userStore = useUserStore();
      userStore.updateUser(userProfile);

      return {
        success: true,
        data: userProfile,
      };
    }

    return response as ApiResponse<UserProfile>;
  }

  async getUsers(
    page?: number,
    pageSize?: number,
    searchTerm?: string,
  ): Promise<
    ApiResponse<{
      items: UserProfile[];
      pageNumber: number;
      pageSize: number;
      totalCount: number;
      totalPages: number;
    }>
  > {
    const response = await this.execute(() => this.client.getUsers(page, pageSize, searchTerm));

    if (response.success && response.data) {
      const { items, pageNumber, pageSize: size, totalCount, totalPages } = response.data;

      return {
        success: true,
        data: {
          items: items?.map(toUserProfile) || [],
          pageNumber: pageNumber || 1,
          pageSize: size || 10,
          totalCount: totalCount || 0,
          totalPages: totalPages || 0,
        },
      };
    }

    return response as ApiResponse<{
      items: UserProfile[];
      pageNumber: number;
      pageSize: number;
      totalCount: number;
      totalPages: number;
    }>;
  }

  async createUser(
    email: string,
    userName: string,
    password: string,
    firstName?: string,
    lastName?: string,
    role?: string,
  ): Promise<ApiResponse<UserProfile>> {
    const dto: CreateUserCommand = {
      email,
      userName,
      password,
      firstName,
      lastName,
      role,
    };

    const response = await this.execute(() => this.client.createUser(dto));

    if (response.success && response.data) {
      return {
        success: true,
        data: toUserProfile(response.data),
      };
    }

    return response as ApiResponse<UserProfile>;
  }

  async updateUser(
    id: string,
    firstName?: string,
    lastName?: string,
    email?: string,
  ): Promise<ApiResponse<UserProfile>> {
    const dto: UpdateUserCommand = {
      userId: id,
      firstName,
      lastName,
      email,
    };

    const response = await this.execute(() => this.client.updateUser(id, dto));

    if (response.success && response.data) {
      return {
        success: true,
        data: toUserProfile(response.data),
      };
    }

    return response as ApiResponse<UserProfile>;
  }

  async getUserById(id: string): Promise<ApiResponse<UserProfile>> {
    const response = await this.execute(() => this.client.getUserById(id));

    if (response.success && response.data) {
      return {
        success: true,
        data: toUserProfile(response.data),
      };
    }

    return response as ApiResponse<UserProfile>;
  }

  async getUserClaims(id: string): Promise<ApiResponse<ClaimDto[]>> {
    const response = await this.execute(() => this.client.getUserClaims(id));

    if (response.success && response.data) {
      return {
        success: true,
        data: response.data,
      };
    }

    return response as ApiResponse<ClaimDto[]>;
  }

  async addUserClaims(id: string, claims: ClaimDto[]): Promise<ApiResponse<ClaimDto[]>> {
    const response = await this.execute(() => this.client.addUserClaims(id, claims));

    if (response.success && response.data) {
      return {
        success: true,
        data: response.data,
      };
    }

    return response as ApiResponse<ClaimDto[]>;
  }

  async removeUserClaims(id: string, claims: ClaimDto[]): Promise<ApiResponse<ClaimDto[]>> {
    const response = await this.execute(() => this.client.removeUserClaims(id, claims));

    if (response.success && response.data) {
      return {
        success: true,
        data: response.data,
      };
    }

    return response as ApiResponse<ClaimDto[]>;
  }

  async addUserRole(id: string, role: string): Promise<ApiResponse<string[]>> {
    const dto: AddUserRoleCommand = { userId: id, role };

    const response = await this.execute(() => this.client.addUserRole(id, dto));

    if (response.success && response.data) {
      return {
        success: true,
        data: response.data.roles ?? [],
      };
    }

    return response as ApiResponse<string[]>;
  }

  async removeUserRole(id: string, role: string): Promise<ApiResponse<string[]>> {
    const response = await this.execute(() => this.client.removeUserRole(id, role));

    if (response.success && response.data) {
      return {
        success: true,
        data: response.data.roles ?? [],
      };
    }

    return response as ApiResponse<string[]>;
  }

  async deleteUser(id: string): Promise<ApiResponse<void>> {
    return this.executeVoid(() => this.client.deleteUser(id));
  }

  async getRoles(): Promise<ApiResponse<string[]>> {
    const response = await this.execute(() => this.client.getRoles());

    if (response.success && response.data) {
      return {
        success: true,
        data: response.data ?? [],
      };
    }

    return response as ApiResponse<string[]>;
  }
}

export const userService = new UserService();
