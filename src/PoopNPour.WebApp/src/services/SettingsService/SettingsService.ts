import { BaseService } from '../BaseService';
import type { ApiResponse } from '../types';
import type { PoopNPourApi } from '@/api/api-client';

/**
 * Settings service for system settings management
 */
class SettingsService extends BaseService {
  /**
   * Get current system settings
   * 
   * @returns Promise with system settings data
   */
  async getSystemSettings(): Promise<ApiResponse<{ setupCompleted: boolean }>> {
    const response = await this.execute(() => this.client.getSystemSettings());

    if (response.success && response.data) {
      return {
        success: true,
        data: {
          setupCompleted: response.data.setupCompleted || false,
        },
      };
    }

    return response as ApiResponse<{ setupCompleted: boolean }>;
  }

  /**
   * Update system settings
   * Requires admin privileges
   * 
   * @param setupCompleted - Whether the setup is completed
   * @returns Promise with void response
   */
  async updateSystemSettings(
    setupCompleted: boolean
  ): Promise<ApiResponse<void>> {
    const dto: PoopNPourApi.UpdateSystemSettingsCommand = {
      setupCompleted,
    };

    return this.executeVoid(() => this.client.updateSystemSettings(dto));
  }
}

export const settingsService = new SettingsService();
