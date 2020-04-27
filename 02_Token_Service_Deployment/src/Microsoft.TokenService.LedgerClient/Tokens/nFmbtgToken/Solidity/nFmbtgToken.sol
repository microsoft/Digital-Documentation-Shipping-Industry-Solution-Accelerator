pragma solidity ^0.5.2;
import "./modified-openzeppelin/token/ERC721/ERC721Basic.sol";
import "./modified-openzeppelin/token/ERC721/ERC721DelegableBasic.sol";
import "./modified-openzeppelin/token/ERC721/ERC721Burnable.sol";
import "./modified-openzeppelin/token/ERC721/ERC721DelegableBurn.sol";
import "./modified-openzeppelin/token/ERC721/ERC721MetadataMintable.sol";
import "./modified-openzeppelin/token/ERC721/ERC721Transferable.sol";
import "./modified-openzeppelin/token/ERC721/ERC721DelegableTransfer.sol";
contract FooToken is ERC721Basic, ERC721DelegableBasic, ERC721Burnable, ERC721DelegableBurn, ERC721MetadataMintable, ERC721Transferable, ERC721DelegableTransfer
{
    address private _owner;

    constructor(string memory name, string memory symbol) ERC721Basic(name, symbol)
    public {
        _owner = msg.sender;
    }
    
}