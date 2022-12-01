import config from '../Base/Config';

export function L(key: string, sourceName?: string): string {
  let localizationSourceName = config.localization.defaultLocalizationSourceName;
  return abp.localization.localize(key, sourceName ? sourceName : localizationSourceName);
}

export function isGranted(permissionName: string): boolean {
  return abp.auth.isGranted(permissionName);
}
