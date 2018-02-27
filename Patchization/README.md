# Patchization
Provides features to execute/create patches and to create very own patching-systems.

## Current Status

At this moment I'm struggling with the IPS' special behaviour...

## ToDo

  - [x] Get rid of IPS' special behaviour
      - [x] Blocksize-limit
      - [x] `EOF`/`0x454F46`-Offset-confusion
  - [x] Provide Creation-Features
  - [ ] Provide an "RLEPatch"-class
  - [x] Provide a BlockInfo-class
  - [ ] Make RHRPatcher great again
  - [ ] Provide generic classes

## Creation-Routine
### Requirements

- [x] Prevent increasement of the "`*.BaseStream.Position`"-value since some streams aren't able to do seek-operations
- [ ] Allowing hash-calculation *(comming soon)*
- [x] Specific BufferSize

Conception
================
## Process
### Patch-Application
 1. Creating a patch based on a stream or a filepath
 2. Reading infos
     1. Reading header-bytes
         1. Checking magic-bytes
     2. Reading blocks
         1. While end of patch isn't reached => reading the next block
         2. Ordering blocks by their position
 3. Applying patch
	 1. Writing blocks using an abstract method

### Patch-Creation
 1. Creating a patch based on a stream
 2. Reading differences and writing them into `BlockInfo`'s
     - [x] Reading buffered (and preventing `BaseStream.Position` and `BaseStream.Length`)
     - [ ] Providing hash-calculation
	     - [ ] **FIX: Using [CryptoStream]**
 3. Ordering blocks by their position
 4. Writing each block
 5. Saving patch using `SaveAs`

 <!--- Rererences -->
 [CryptoStream]: https://msdn.microsoft.com/de-de/library/system.security.cryptography.cryptostream(v=vs.110).aspx