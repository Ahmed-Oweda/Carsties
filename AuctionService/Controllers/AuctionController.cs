﻿using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionController : ControllerBase
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;
        public AuctionController(AuctionDbContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDTO>>> GetAllAuctions(string date)
        {
            //var auctions = await _context.Auctions
            //                .Include(x => x.Item)
            //                .OrderBy(x => x.Item.Make)
            //                .ToListAsync();
            //return _mapper.Map<List<AuctionDTO>>(auctions);

            var query = _context.Auctions.OrderBy(x=>x.Item.Make).AsQueryable();
            if(!string.IsNullOrEmpty(date))
                query = query.Where(x=>x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
            return await query.ProjectTo<AuctionDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDTO>> GetAuctionById(Guid Id)
        {
            var auction = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (auction == null) return NotFound();
            return _mapper.Map<AuctionDTO>(auction);
        }

        [HttpPost]
        public async Task<ActionResult<AuctionDTO>> CreateAuction(CreateAuctionDTO auctionDto)
        {
            var auction = _mapper.Map<Auction>(auctionDto);
            //Todo : add current user as seller 
            auction.Seller = "test";
            _context.Auctions.Add(auction);
            var result = await _context.SaveChangesAsync() > 0;
            if (!result) return BadRequest("can't save changes to the DB");

            return CreatedAtAction(nameof(GetAuctionById), new { auction.Id }, _mapper.Map<AuctionDTO>(auction));

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid id , UpdateAuctionDTO updateAuctionDto)
        {
            var auction = await _context.Auctions.Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (auction == null)
                return NotFound();

            //Todo check seller = username 
            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

            var result = await _context.SaveChangesAsync() > 0;
            if (result) return Ok();
            return BadRequest("problem saving changes");

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null) return NotFound();

            //Todo check seller = username 
            _context.Auctions.Remove(auction);

            var result = await _context.SaveChangesAsync() > 0;
            if (result) return Ok();
            return BadRequest("problem deleting auction");
        }
    }
}
