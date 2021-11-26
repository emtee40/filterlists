﻿export interface List {
  id: number;
  name: string;
  description?: string;
  licenseId?: number;
  syntaxIds: number[];
  languageIds: number[];
  tagIds: number[];
  maintainerIds: number[];

  // auto-generated by useLists hook
  slug: string;
}
