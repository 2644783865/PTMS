import { BlockTypeDto } from './BlockTypeDto';

export interface GranitDto {
  id: number
  blockNumber: number
  blockTypeId: number
  objectId: number

  blockType: BlockTypeDto
}
